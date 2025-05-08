using Game;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Boss boss;
    private NavMeshAgent agent;
    private Animator anim;
    private BoxCollider colliders;

    [Header("Target Detection")]
    public float viewDistance = 10f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public Transform target;
    public bool isPerson;

    [Header("Phase")]
    public BossPhase phase = BossPhase.Phase1;
    public BossState? pendingState = null;

    [Header("Skill Cooldowns & Flags")]
    public bool isSkillActive = false;
    [SerializeField] private float postSkillCooldown = 60f;
    [SerializeField] private float postSkillCooldownTimer = 0f;
    [SerializeField] private bool phase3SkillUsed = false;
    [SerializeField] private bool phase4SkillUsed = false;

    [Header("Skill 3 Settings")]
    [SerializeField] private float flyUpAmount;
    [SerializeField] private float flyUpDuration;
    [SerializeField] private float rotateAngleX;

    [Header("Skill Effects")]
    public GameObject skillEff1;

    private GameObject currentSkillArea;

    [Header("스킬 영역 데이터")]
    public SkillAreaData skill1Data;
    public SkillAreaData skill2Data;
    public SkillAreaData skill3Data;

    // ─────────────────────────────────────────────
    // ▶ 초기화
    void Awake()
    {
        boss = GetComponent<Boss>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        colliders = GetComponent<BoxCollider>();
        postSkillCooldownTimer = postSkillCooldown;
    }

    void Update()
    {
        HandleSkills();
    }

    // ─────────────────────────────────────────────
    // ▶ 스테이트 / 페이즈 관리

    public void UpdatePhase(float currentHP, float maxHP)
    {
        float hpRatio = currentHP / maxHP;

        if (hpRatio <= 0.2f) phase = BossPhase.Phase4;
        else if (hpRatio <= 0.5f) phase = BossPhase.Phase3;
        else if (hpRatio <= 0.8f) phase = BossPhase.Phase2;
        else phase = BossPhase.Phase1;
    }

    public void HandleSkills()
    {
        if (phase == BossPhase.Phase1) return;

        postSkillCooldownTimer += Time.deltaTime;

        if (postSkillCooldownTimer >= postSkillCooldown && !isSkillActive)
        {
            postSkillCooldownTimer = 0f;
            SendSkillPacket(BossState.Skill1);
        }

        if (phase == BossPhase.Phase3 && !phase3SkillUsed)
        {
            phase3SkillUsed = true;
            SendSkillPacket(BossState.Skill2);
        }

        if (phase == BossPhase.Phase4 && !phase4SkillUsed)
        {
            phase4SkillUsed = true;
            SendSkillPacket(BossState.Skill3);
        }
    }

    private void SendSkillPacket(BossState skillState)
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE
        {
            BossID = boss.bossID,
            BossState = (uint)skillState
        };
        Managers.Network.Send(packet);
    }

    // ─────────────────────────────────────────────
    // ▶ 네비게이션 / 추적

    public void CSChaseTarget()
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE
        {
            BossID = boss.bossID,
            BossState = (int)BossState.Chase,
            TargetMovementPos = new Position
            {
                PosX = target.position.x,
                PosY = target.position.y,
                PosZ = target.position.z
            },
            CurSpeed = boss.SOData.GroundData.BaseSpeed * boss.SOData.GroundData.ChasingSpeedModifier
        };
        Managers.Network.Send(packet);
    }

    public void SCChaseTarget(Vector3 destination)
    {
        agent.SetDestination(destination);
    }

    public void SCMoveSpeed(float modifier)
    {
        agent.speed = modifier * 3.8f;
    }

    public bool IsAttackRange(BossData data)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= data.AttackRange;
    }

    public bool DetectTargets(float distance)
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, distance, targetMask);
        if (hits.Length > 0)
        {
            if (target == null)
                target = hits[0].transform;
            return true;
        }
        return false;
    }

    // ─────────────────────────────────────────────
    // ▶ 스킬 실행

    public void UseSkill3(Transform trans)
    {
        ShowSkill3Area();
        StartCoroutine(FlyUp(trans));
    }

    private IEnumerator FlyUp(Transform target)
    {
        Vector3 startPos = target.localPosition;
        Vector3 endPos = startPos + Vector3.up * flyUpAmount;

        Quaternion startRot = target.localRotation;
        Quaternion endRot = Quaternion.Euler(rotateAngleX, startRot.eulerAngles.y, startRot.eulerAngles.z);

        float elapsed = 0f;

        while (elapsed < flyUpDuration)
        {
            float t = elapsed / flyUpDuration;
            target.localPosition = Vector3.Lerp(startPos, endPos, t);
            target.localRotation = Quaternion.Lerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localPosition = endPos;
        target.localRotation = endRot;
        anim.SetBool("Skill3Attack", true);
    }

    public void EndSkill3(Transform trans)
    {
        StartCoroutine(FlyDown(trans));
    }

    private IEnumerator FlyDown(Transform target)
    {
        Vector3 startPos = target.localPosition;
        Vector3 endPos = startPos + Vector3.down * flyUpAmount;

        Quaternion startRot = target.localRotation;
        Quaternion endRot = Quaternion.Euler(startRot.eulerAngles.x - rotateAngleX, startRot.eulerAngles.y, startRot.eulerAngles.z);

        float elapsed = 0f;

        while (elapsed < flyUpDuration)
        {
            float t = elapsed / flyUpDuration;
            target.localPosition = Vector3.Lerp(startPos, endPos, t);
            target.localRotation = Quaternion.Lerp(startRot, endRot, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        target.localPosition = endPos;
        target.localRotation = endRot;
        anim.SetBool("Skill3Down", true);
    }

    public void OnSkill2Eff(float delay)
    {
        skillEff1.SetActive(true);
        ShowSkill2Area();
        StartCoroutine(DeactivateEff(skillEff1, delay));
    }

    private IEnumerator DeactivateEff(GameObject eff, float delay)
    {
        yield return new WaitForSeconds(delay);
        eff.SetActive(false);
    }

    // ─────────────────────────────────────────────
    // ▶ 스킬 범위 표시 (UI)

    public void ShowSkill1Area()
    {
        CleanupSkillArea();

        Vector3 center = transform.position + skill1Data.offset;
        currentSkillArea = new GameObject("SkillAreaContainer");
        currentSkillArea.transform.position = center;
        currentSkillArea.transform.SetParent(transform);

        Vector3[] dirs = { transform.forward, -transform.forward, transform.right, -transform.right };

        foreach (var dir in dirs)
        {
            Vector3 spawnPos = center + dir * skill1Data.radius;
            GameObject cone = Instantiate(skill1Data.conePrefab, spawnPos, Quaternion.LookRotation(dir), currentSkillArea.transform);
            cone.transform.localScale = Vector3.one * skill1Data.radius;
        }

        StartCoroutine(HideSkillAreaAfter(skill1Data.duration));
    }


    public void ShowSkill2Area()
    {
        CleanupSkillArea();

        Vector3 spawnPos = transform.position + skill2Data.offset;
        currentSkillArea = Instantiate(skill2Data.conePrefab, spawnPos, Quaternion.identity, transform);
        currentSkillArea.transform.localScale = Vector3.one * skill2Data.radius;

        StartCoroutine(HideSkillAreaAfter(skill2Data.duration));
    }

    public void ShowSkill3Area()
    {
        CleanupSkillArea();

        Vector3 center = transform.position + skill3Data.offset;
        float scale = skill3Data.radius;

        currentSkillArea = new GameObject("SkillAreaContainer");
        currentSkillArea.transform.position = center;
        currentSkillArea.transform.SetParent(transform);

        GameObject baseArea = Instantiate(skill3Data.areaBasePrefab, center, Quaternion.identity, currentSkillArea.transform);
        baseArea.transform.localScale = Vector3.one * scale;

        GameObject growArea = Instantiate(skill3Data.areaGrowPrefab, center, Quaternion.identity, currentSkillArea.transform);
        growArea.transform.localScale = Vector3.zero;

        if (skill3Data.useGrowEffect)
            StartCoroutine(ScaleOverTime(growArea, 0f, scale, skill3Data.growTime));

        StartCoroutine(HideSkillAreaAfter(skill3Data.duration));
    }

    private IEnumerator ScaleOverTime(GameObject target, float fromXZ, float toXZ, float duration)
    {
        float elapsed = 0f;
        Vector3 from = new Vector3(fromXZ, target.transform.localScale.y, fromXZ);
        Vector3 to = new Vector3(toXZ, target.transform.localScale.y, toXZ);

        while (elapsed < duration)
        {
            if (target == null) yield break;
            target.transform.localScale = Vector3.Lerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        if (target != null)
            target.transform.localScale = to;
    }

    private IEnumerator HideSkillAreaAfter(float delay)
    {
        yield return new WaitForSeconds(delay);
        CleanupSkillArea();
    }

    private void CleanupSkillArea()
    {
        if (currentSkillArea != null)
            Destroy(currentSkillArea);
    }

    // ─────────────────────────────────────────────
    // ▶ 애니메이션 이벤트

    public void EndSkillAnim()
    {
        StartCoroutine(RemoveSkillBools());
    }

    private IEnumerator RemoveSkillBools()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Skill3Attack", false);
        anim.SetBool("Skill3Down", false);
    }

    public void ColliderOnEnable(float delay)
    {
        colliders.enabled = true;
        Debug.Log("Collider On");
        StartCoroutine(DisableCollider(delay));
    }

    IEnumerator DisableCollider(float delay)
    {
        yield return new WaitForSeconds(delay);
        colliders.enabled = false;
        Debug.Log("Collider Off");
    }

    // ─────────────────────────────────────────────
    // ▶ 디버그

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance);
    }
}

//public void StartWalk()
//{
//    //wanderTarget = GetRandomWalkPoint(2f, 4f);
//    agent.SetDestination(wanderTarget);
//}

//private Vector3 GetRandomWalkPoint(float minRaius, float maxRaius)
//{
//    Vector3 randomPoint = Random.insideUnitSphere * Random.Range(minRaius, maxRaius);
//    randomPoint += transform.position;

//    NavMeshHit hit;
//    if (NavMesh.SamplePosition(randomPoint, out hit, maxRaius, NavMesh.AllAreas))
//    {
//        return hit.position;
//    }

//    return transform.position; // 기본 위치 반환
//}

//public bool EndWalk()
//{
//    return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
//}

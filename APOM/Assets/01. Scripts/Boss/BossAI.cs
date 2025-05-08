using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using SEnum = System.Enum;

public class BossAI : MonoBehaviour
{
    private Boss boss;
    [Header("타겟 탐지 설정")]
    public float viewDistance = 10f;
    public LayerMask targetMask;
    public LayerMask obstacleMask; // 장애물 레이어 (벽 등)

    private Vector3 wanderTarget;

    private NavMeshAgent agent;
    public Transform target;
    public bool isPerson;

    [Header("페이즈 설정")]
    public BossPhase phase = BossPhase.Phase1;

    private float _moveSpeed = 0f;

    private BoxCollider colliders;
    private Animator anim;

    public GameObject skillEff1;

    [Header("쿨타임, 1회성 제한")]
    [SerializeField] private float postSkillCooldown = 60f;
    [SerializeField] private float postSkillCooldownTimer = 0f;
    [SerializeField] private bool phase3SkillUsed = false;
    [SerializeField] private bool phase4SkillUsed = false;

    [Header("스킬3 설정")]
    [SerializeField] private float flyUpAmount;
    [SerializeField] private float flyUpDuration;
    [SerializeField] private float rotateAngleX;

    public bool isSkillActive = false;

    private GameObject currentSkillArea;
    public GameObject skillConePrefab; // 스킬 범위 표시용 콘 모양 프리팹
    public GameObject skillAreaPrefab; // 스킬 범위 표시용 범위 모양 프리팹
    public GameObject skillAreaFillPrefab; // 스킬 범위 표시용 커지는 모양 프리팹


    //Dictionary<BossPhase, List<BossSkill>> bossSkillData = new();

    void Awake()
    {
        boss = GetComponent<Boss>();
        agent = GetComponent<NavMeshAgent>();
        colliders = GetComponent<BoxCollider>();
        anim = GetComponentInChildren<Animator>();
        postSkillCooldownTimer = postSkillCooldown;
    }


    private void Update()
    {
        HandleSkills();
        //DetectTargets(boss.SOData.PlayerChasingRange);
    }

    public void SCMoveSpeed(float Modifier)
    {
        agent.speed = Modifier * 3.8f;
    }

    // 플레이어 추적
    public void CSChaseTarget()
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
        packet.BossID = boss.bossID;
        packet.BossState = (int)BossState.Chase;
        packet.TargetMovementPos = new Position 
        { PosX = target.position.x, PosY = target.position.y, PosZ = target.position.z };
        packet.CurSpeed = boss.SOData.GroundData.BaseSpeed * boss.SOData.GroundData.ChasingSpeedModifier;
        Managers.Network.Send(packet);
    }

    public void SCChaseTarget(Vector3 target)
    {
        agent.SetDestination(target);
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

    public void OnSkill2Eff(float delay)
    {
        skillEff1.SetActive(true);
        Vector3 up = new Vector3(0, 2, 0);
        ShowSkill2Area(transform.position + up, 2f, 1f); // 스킬 범위 표시
        StartCoroutine(StartEffSkill2(delay));
    }

    IEnumerator StartEffSkill2(float delay)
    {
        yield return new WaitForSeconds(delay);
        skillEff1.SetActive(false);
    }

    public void UseSkill3(Transform transform)
    {
        Vector3 up = new Vector3(0, 2, 0);
        ShowSkill3Area(transform.position + up, 3f, 10f);
        StartCoroutine(FlyUp(transform));
    }

    IEnumerator FlyUp(Transform target)
    {
        Vector3 startPos = target.localPosition;
        Vector3 endPos = startPos + Vector3.up * flyUpAmount;

        Quaternion startRot = target.localRotation;
        Quaternion endRot = Quaternion.Euler
            (rotateAngleX,
            startRot.eulerAngles.y,
            startRot.eulerAngles.z);

        float elapsedTime = 0f;

        while (elapsedTime < flyUpDuration)
        {
            float t = elapsedTime / flyUpDuration;

            target.localPosition = Vector3.Lerp(startPos, endPos, t);
            target.localRotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localPosition = endPos;
        target.localRotation = endRot;
        anim.SetBool("Skill3Attack", true);
    }

    public void EndSkill3(Transform transform)
    {
        StartCoroutine(FlyDown(transform));
    }

    IEnumerator FlyDown(Transform target)
    {
        Vector3 startPos = target.localPosition;
        Vector3 endPos = startPos + Vector3.down * flyUpAmount;

        Quaternion startRot = target.localRotation;
        Quaternion endRot = Quaternion.Euler
            (startRot.eulerAngles.x - rotateAngleX,
            startRot.eulerAngles.y,
            startRot.eulerAngles.z);

        float elapsedTime = 0f;

        while (elapsedTime < flyUpDuration)
        {
            float t = elapsedTime / flyUpDuration;

            target.localPosition = Vector3.Lerp(startPos, endPos, t);
            target.localRotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        target.localPosition = endPos;
        target.localRotation = endRot;
        anim.SetBool("Skill3Down", true);
    }
    public void ShowSkill1Area(Vector3 position, float radius, float duration)
    {
        if (currentSkillArea != null)
            Destroy(currentSkillArea);

        currentSkillArea = new GameObject("SkillAreaContainer");
        currentSkillArea.transform.position = position;
        currentSkillArea.transform.SetParent(transform);

        Vector3[] directions =
            { transform.forward, -transform.forward, transform.right, -transform.right};

        foreach (Vector3 dir in directions)
        {
            Vector3 spawnPos = position + dir.normalized * radius;

            GameObject cone = Instantiate(skillConePrefab, spawnPos, Quaternion.LookRotation(dir), currentSkillArea.transform);
            cone.transform.SetParent(currentSkillArea.transform);

            // 크기 조정 (Canvas scale 기준)
            float scale = radius * 2f;
            cone.transform.localScale = new Vector3(scale, scale, scale);
        }

        StartCoroutine(HideSkillAreaAfter(duration));
    }
    public void ShowSkill2Area(Vector3 position, float radius, float duration)
    {
        if (currentSkillArea != null)
            Destroy(currentSkillArea);

        currentSkillArea = Instantiate(skillConePrefab, position, Quaternion.identity);
        currentSkillArea.transform.SetParent(transform);

        // 크기 조정 (Canvas scale 기준)
        float scale = radius * 2f;
        currentSkillArea.transform.localScale = new Vector3(scale, scale, scale);

        StartCoroutine(HideSkillAreaAfter(duration));
    }

    public void ShowSkill3Area(Vector3 position, float radius, float duration)
    {
        if (currentSkillArea != null)
            Destroy(currentSkillArea);

        currentSkillArea = new GameObject("SkillAreaContainer");
        currentSkillArea.transform.position = position;
        currentSkillArea.transform.SetParent(transform);

        float baseScale = radius * 2f;

        // 1. 프리팹 A - 고정 크기
        GameObject areaA = Instantiate(skillAreaPrefab, position, Quaternion.identity, currentSkillArea.transform);
        areaA.transform.localScale = new Vector3(baseScale, baseScale, baseScale);

        // 2. 프리팹 B - 커지는 효과
        GameObject areaB = Instantiate(skillAreaFillPrefab, position, Quaternion.identity, currentSkillArea.transform);
        areaB.transform.localScale = new Vector3(0, 0, 0);

        StartCoroutine(ScaleOverTime(areaB, 0f, baseScale, duration)); // x,z만 커짐

        StartCoroutine(HideSkillAreaAfter(duration));
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
        if (currentSkillArea != null)
            Destroy(currentSkillArea);
    }

    public void EndSkillAnim()
    {
        StartCoroutine(RemoveSetBool());
    }

    IEnumerator RemoveSetBool()
    {
        yield return new WaitForSeconds(0.5f);
        anim.SetBool("Skill3Attack", false);
        anim.SetBool("Skill3Down", false);
    }

    //스킬 쿨타임 및 발동 처리
    public void HandleSkills()
    {
        if (phase == BossPhase.Phase1)
            return;

        //  Skill2 쿨타임마다 반복
        postSkillCooldownTimer += Time.deltaTime;

        if (postSkillCooldownTimer >= postSkillCooldown)
        {
            postSkillCooldownTimer = 0f;

            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = boss.bossID;
            packet.BossState = (int)BossState.Skill1;
            Managers.Network.Send(packet);
        }

        //  Phase3 진입 시 Skill3 1회 발동
        if (phase == BossPhase.Phase3 && !phase3SkillUsed)
        {
            phase3SkillUsed = true;

            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = boss.bossID;
            packet.BossState = (int)BossState.Skill2;
            Managers.Network.Send(packet);
        }

        //  Phase4 진입 시 Skill4 1회 발동
        if (phase == BossPhase.Phase4 && !phase4SkillUsed)
        {
            phase4SkillUsed = true;

            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = boss.bossID;
            packet.BossState = (int)BossState.Skill3;
            Managers.Network.Send(packet);
        }
    }

    //보스의 현재 HP 상태에 따라 페이즈 전환
    public void UpdatePhase(float currentHP, float maxHP)
    {
        float hpRatio = currentHP / maxHP;

        if (hpRatio <= 0.2f)
            phase = BossPhase.Phase4;
        else if (hpRatio <= 0.5f)
            phase = BossPhase.Phase3;
        else if (hpRatio <= 0.8f)
            phase = BossPhase.Phase2;
        else
            phase = BossPhase.Phase1;
    }

     //360도 시야 범위 내에서 타겟을 탐지
    public bool DetectTargets(float distance)
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, distance, targetMask);

        foreach (Collider targetCollider in targetsInRange)
        {
            if (target == null)
            {
                target = targetCollider.transform;
            }
            return true;
        }
        return false;
    }

    // 시각적으로 시야 범위를 확인하기 위한 Gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance); // 시야 범위
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance); // 시야의 앞쪽 (시작선)
    }

    public bool IsAttackRange(BossData data)
    {
        if (target == null) return false;

        // 타겟과의 거리 계산
        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= data.AttackRange;
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

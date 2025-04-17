using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using SEnum = System.Enum;

//[System.Serializable]
//public class BossSkill
//{
//    public float skillTimer = 0f;
//    public float skillDelay;         // 쿨타임
//    public BossPhase phase;          // 이 스킬이 등장하는 시작 페이즈
//    public BossSkillType skillType = BossSkillType.None; // 스킬 타입
//    public int animationHash; // 애니메이션 해시
//}
public class BossAI : MonoBehaviour
{
    [Header("타겟 탐지 설정")]
    public float viewDistance = 10f;
    public LayerMask targetMask;
    public LayerMask obstacleMask; // 장애물 레이어 (벽 등)

    private Vector3 wanderTarget;

    private NavMeshAgent agent;
    public Transform target;

    [Header("페이즈 설정")]
    public BossPhase phase = BossPhase.Phase1;

    //[Header("스킬 리스트")]
    //public List<BossSkill> skillList;
    //private BossSkill nextSkillToUse; // 다음 사용할 스킬
    //public BossSkill NextSkillToUse
    //{
    //    get => nextSkillToUse; // 외부에서 접근할 수 있도록 프로퍼티로 제공
    //    set => nextSkillToUse = value;
    //}

    //[HideInInspector] public float postSkillCooldown = 1f; // 스킬 종료 후 대기 시간
    //[HideInInspector] public float postSkillCooldownTimer = 0f;

    //Dictionary<BossPhase, List<BossSkill>> bossSkillData = new();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //InitializeSkillDictionary();
    }

    //private void InitializeSkillDictionary()
    //{
    //    // 딕셔너리 초기화
    //    foreach (BossPhase phase in SEnum.GetValues(typeof(BossPhase)))
    //    {
    //        bossSkillData[phase] = new List<BossSkill>();
    //    }

    //    // skillList 안에서 각 스킬의 phase 기준으로 분류
    //    foreach (var skill in skillList)
    //    {
    //        bossSkillData[skill.phase].Add(skill);
    //    }
    //}

    //public void InitSkillsAnimationHash(BossAnimationData animData)
    //{
    //    foreach (var skill in skillList)
    //    {
    //        switch (skill.skillType)
    //        {
    //            case BossSkillType.Skill1:
    //                skill.animationHash = animData.BossSkill1ParameterHash;
    //                break;
    //            case BossSkillType.Skill2:
    //                skill.animationHash = animData.BossSkill2ParameterHash;
    //                break;
    //            case BossSkillType.Skill3:
    //                skill.animationHash = animData.BossSkill3ParameterHash;
    //                break;
    //        }
    //    }
    //}


    // 스킬 쿨타임 및 발동 처리
    //public void HandleSkills()
    //{
    //    // 스킬이 하나 실행 중이면 더 이상 진행하지 않음
    //    if (nextSkillToUse != null) return;

    //    // 쿨다운 타이머가 남아있으면 아무 스킬도 선택하지 않음
    //    if (postSkillCooldownTimer > 0f)
    //    {
    //        postSkillCooldownTimer -= Time.deltaTime;
    //        return;
    //    }

    //    foreach (BossSkill skill in GetAllSkillsUpToCurrentPhase())
    //    {
    //        skill.skillTimer += Time.deltaTime;

    //        if (skill.skillTimer >= skill.skillDelay)
    //        {
    //            nextSkillToUse = skill;
    //            skill.skillTimer = 0f;
    //            break;
    //        }
    //    }
    //}

    //public void ClearSkill()
    //{
    //    NextSkillToUse = null;
    //    postSkillCooldownTimer = postSkillCooldown;
    //}

    //private void UseSkill(BossSkill skill)
    //{
    //    Debug.Log($"[페이즈 {phase}] 스킬 발동!");
    //    // 이펙트 및 Transform 처리? 정도 
    //}

    // 현재 페이즈까지의 모든 스킬을 가져오는 메서드
    //public List<BossSkill> GetAllSkillsUpToCurrentPhase()
    //{
    //    List<BossSkill> result = new();

    //    foreach (BossPhase p in SEnum.GetValues(typeof(BossPhase)))
    //    {
    //        if (p <= phase && bossSkillData.ContainsKey(p))
    //        {
    //            result.AddRange(bossSkillData[p]);
    //        }
    //    }

    //    return result;
    //}


    // 보스의 현재 HP 상태에 따라 페이즈 전환
    //public void UpdatePhase(float currentHP, float maxHP)
    //{
    //    float hpRatio = currentHP / maxHP;

    //    if (hpRatio <= 0.2f)
    //        phase = BossPhase.Phase4;
    //    else if (hpRatio <= 0.7f)
    //        phase = BossPhase.Phase3;
    //    else if (hpRatio <= 0.8f)
    //        phase = BossPhase.Phase2;
    //    else
    //        phase = BossPhase.Phase1;
    //}

    // 360도 시야 범위 내에서 타겟을 탐지
    //public bool DetectTargets()
    //{
    //    Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

    //    foreach (Collider targetCollider in targetsInRange)
    //    {
    //        if (target == null)
    //        {
    //            target = targetCollider.transform;
    //        }
    //        return true;
    //    }
    //    return false;
    //}

    public void MoveSpeed(float Modifier)
    {
        agent.speed = Modifier;
    }

    // 플레이어 추적
    public void ChaseTarget(Vector3 target)
    {

            //타겟 위치로 네브메시 에이전트가 이동
            agent.SetDestination(target);

    }

    //// 시각적으로 시야 범위를 확인하기 위한 Gizmo
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, viewDistance); // 시야 범위
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance); // 시야의 앞쪽 (시작선)
    //}

    //public bool IsAttackRange(BossData data)
    //{
    //    if (target == null) return false;

    //    // 타겟과의 거리 계산
    //    float distance = Vector3.Distance(transform.position, target.position);
    //    return distance <= data.AttackRange;
    //}

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
}

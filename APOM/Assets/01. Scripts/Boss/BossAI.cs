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
//    public float skillDelay;         // ��Ÿ��
//    public BossPhase phase;          // �� ��ų�� �����ϴ� ���� ������
//    public BossSkillType skillType = BossSkillType.None; // ��ų Ÿ��
//    public int animationHash; // �ִϸ��̼� �ؽ�
//}
public class BossAI : MonoBehaviour
{
    [Header("Ÿ�� Ž�� ����")]
    public float viewDistance = 10f;
    public LayerMask targetMask;
    public LayerMask obstacleMask; // ��ֹ� ���̾� (�� ��)

    private Vector3 wanderTarget;

    private NavMeshAgent agent;
    public Transform target;

    [Header("������ ����")]
    public BossPhase phase = BossPhase.Phase1;

    //[Header("��ų ����Ʈ")]
    //public List<BossSkill> skillList;
    //private BossSkill nextSkillToUse; // ���� ����� ��ų
    //public BossSkill NextSkillToUse
    //{
    //    get => nextSkillToUse; // �ܺο��� ������ �� �ֵ��� ������Ƽ�� ����
    //    set => nextSkillToUse = value;
    //}

    //[HideInInspector] public float postSkillCooldown = 1f; // ��ų ���� �� ��� �ð�
    //[HideInInspector] public float postSkillCooldownTimer = 0f;

    //Dictionary<BossPhase, List<BossSkill>> bossSkillData = new();

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        //InitializeSkillDictionary();
    }

    //private void InitializeSkillDictionary()
    //{
    //    // ��ųʸ� �ʱ�ȭ
    //    foreach (BossPhase phase in SEnum.GetValues(typeof(BossPhase)))
    //    {
    //        bossSkillData[phase] = new List<BossSkill>();
    //    }

    //    // skillList �ȿ��� �� ��ų�� phase �������� �з�
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


    // ��ų ��Ÿ�� �� �ߵ� ó��
    //public void HandleSkills()
    //{
    //    // ��ų�� �ϳ� ���� ���̸� �� �̻� �������� ����
    //    if (nextSkillToUse != null) return;

    //    // ��ٿ� Ÿ�̸Ӱ� ���������� �ƹ� ��ų�� �������� ����
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
    //    Debug.Log($"[������ {phase}] ��ų �ߵ�!");
    //    // ����Ʈ �� Transform ó��? ���� 
    //}

    // ���� ����������� ��� ��ų�� �������� �޼���
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


    // ������ ���� HP ���¿� ���� ������ ��ȯ
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

    // 360�� �þ� ���� ������ Ÿ���� Ž��
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

    // �÷��̾� ����
    public void ChaseTarget(Vector3 target)
    {

            //Ÿ�� ��ġ�� �׺�޽� ������Ʈ�� �̵�
            agent.SetDestination(target);

    }

    //// �ð������� �þ� ������ Ȯ���ϱ� ���� Gizmo
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, viewDistance); // �þ� ����
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance); // �þ��� ���� (���ۼ�)
    //}

    //public bool IsAttackRange(BossData data)
    //{
    //    if (target == null) return false;

    //    // Ÿ�ٰ��� �Ÿ� ���
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

    //    return transform.position; // �⺻ ��ġ ��ȯ
    //}

    //public bool EndWalk()
    //{
    //    return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    //}
}

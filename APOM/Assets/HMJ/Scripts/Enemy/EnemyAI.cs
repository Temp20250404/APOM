using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float viewDistance = 10f;
    public float viewAngle = 360f;
    public LayerMask targetMask;
    public LayerMask obstacleMask; // ��ֹ� ���̾� (�� ��)

    private float waitTimer;
    public bool isWaiting = false;
    private float nextWaitDuration;
    private Vector3 wanderTarget;

    private NavMeshAgent agent;
    public Transform target;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // 360�� �þ� ���� ������ Ÿ���� Ž��
    public bool DetectTargets()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

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

    public void MoveSpeed(float Modifier)
    {
        agent.speed = Modifier;
        Debug.Log("MoveSpeed : " + agent.speed);
    }

    // �÷��̾� ����
    public void ChaseTarget()
    {
        if (target != null)
        {
            // Ÿ�� ��ġ�� �׺�޽� ������Ʈ�� �̵�
            agent.SetDestination(target.position);
        }
    }

    // �ð������� �þ� ������ Ȯ���ϱ� ���� Gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance); // �þ� ����
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance); // �þ��� ���� (���ۼ�)
    }

    public bool IsAttackRange(EnemyData data)
    {
        if (target == null) return false;

        // Ÿ�ٰ��� �Ÿ� ���
        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= data.AttackRange;
    }

    public void StartWalk()
    {
        isWaiting = false;
        waitTimer = 0f;
        nextWaitDuration = Random.Range(3f, 6f);

        wanderTarget = GetRandomWalkPoint(2f, 4f);
        agent.SetDestination(wanderTarget);
    }

    private Vector3 GetRandomWalkPoint(float minRaius, float maxRaius)
    {
        Vector3 randomPoint = Random.insideUnitSphere * Random.Range(minRaius, maxRaius);
        randomPoint += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, maxRaius, NavMesh.AllAreas))
        {
            return hit.position;
        }

        return transform.position; // �⺻ ��ġ ��ȯ
    }

    public bool EndWalk()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}

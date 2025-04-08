using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float viewDistance = 10f;
    public float viewAngle = 360f;
    public LayerMask targetMask;
    public LayerMask obstacleMask; // 장애물 레이어 (벽 등)

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

    // 360도 시야 범위 내에서 타겟을 탐지
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

    // 플레이어 추적
    public void ChaseTarget()
    {
        if (target != null)
        {
            // 타겟 위치로 네브메시 에이전트가 이동
            agent.SetDestination(target.position);
        }
    }

    // 시각적으로 시야 범위를 확인하기 위한 Gizmo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewDistance); // 시야 범위
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * viewDistance); // 시야의 앞쪽 (시작선)
    }

    public bool IsAttackRange(EnemyData data)
    {
        if (target == null) return false;

        // 타겟과의 거리 계산
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

        return transform.position; // 기본 위치 반환
    }

    public bool EndWalk()
    {
        return !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance;
    }
}

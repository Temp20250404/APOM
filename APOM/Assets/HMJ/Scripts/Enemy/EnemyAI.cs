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

    private NavMeshAgent agent;
    public Transform target;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // 360�� �þ� ���� ������ Ÿ���� Ž��
    public bool DetectTargets()
    {
        Collider[] targetsInRange = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        foreach (Collider targetCollider in targetsInRange)
        {
            if(target == null)
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
}

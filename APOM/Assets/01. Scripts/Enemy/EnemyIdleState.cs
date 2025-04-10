using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    private float idleTime;
    private float waitTime;
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Idle ���¿� ������ ��
    public override void StateEnter()
    {
        // ������ �ִ� �����̱� ������ Speed�� 0����
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        // Animation ��ȯ
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.IdleParameterHash);

        idleTime = Random.Range(2f, 4f);
        waitTime = 0f;
    }

    // Idle ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void StateExit()
    {
        base.StateExit();

        StopAnimation(stateMachine.Enemy.EnemyAnimationData.IdleParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (stateMachine.Enemy.enemyAI.IsAttackRange(stateMachine.Enemy.SOData))
        {
            AnimatorStateInfo animStateInfo = stateMachine.Enemy.Anim.GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsTag("Idle") && animStateInfo.normalizedTime >= 0.8f)
            {
                stateMachine.ChangeState(EnemyState.Attack);
                Debug.Log("Attack State");  
            }
        }

        if (!stateMachine.Enemy.enemyAI.IsAttackRange(stateMachine.Enemy.SOData) && stateMachine.Enemy.enemyAI.DetectTargets())
        {
            stateMachine.ChangeState(EnemyState.Chase);
            Debug.Log("Chase State");
        }

        UpdateWalk();
    }

    private void UpdateWalk()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= idleTime)
        {
            stateMachine.ChangeState(EnemyState.Walk);
            Debug.Log("Walk State");
        }
    }
}
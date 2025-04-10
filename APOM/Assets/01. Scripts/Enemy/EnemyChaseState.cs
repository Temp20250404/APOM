using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk ���·� ��ȯ�Ǿ��� ��
    public override void StateEnter()
    {
        // BaseSpeed�� ������ �� ����
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.StateEnter();
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.ChasingParameterHash);
    }

    // Walk ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Enemy.EnemyAnimationData.ChasingParameterHash);
    }

    public override void StateUpdate()
    {
        if(stateMachine.Enemy.enemyAI.DetectTargets())
        {
            stateMachine.Enemy.enemyAI.ChaseTarget();
        }
        else
        {
            stateMachine.Enemy.enemyAI.target = null;
            stateMachine.ChangeState(EnemyState.Idle);
        }

        if (stateMachine.Enemy.enemyAI.IsAttackRange(stateMachine.Enemy.SOData))
        {
            stateMachine.ChangeState(EnemyState.Attack);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk ���·� ��ȯ�Ǿ��� ��
    public override void Enter()
    {
        // BaseSpeed�� ������ �� ����
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.Enter();
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.ChasingParameterHash);
    }

    // Walk ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.EnemyAnimationData.ChasingParameterHash);
    }

    public override void Update()
    {
        if(stateMachine.Enemy.enemyAI.DetectTargets())
        {
            stateMachine.Enemy.enemyAI.ChaseTarget();
        }
        else
        {
            stateMachine.Enemy.enemyAI.target = null;
            stateMachine.ChangeState(stateMachine.EnemyIdleState);
        }
    }
}

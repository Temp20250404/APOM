using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk 상태로 전환되었을 때
    public override void StateEnter()
    {
        // BaseSpeed에 곱해줄 값 세팅
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.StateEnter();
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.ChasingParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
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

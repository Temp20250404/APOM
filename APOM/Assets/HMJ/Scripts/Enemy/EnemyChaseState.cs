using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : EnemyBaseState
{
    public EnemyChaseState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk 상태로 전환되었을 때
    public override void Enter()
    {
        // BaseSpeed에 곱해줄 값 세팅
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.Enter();
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.ChasingParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
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

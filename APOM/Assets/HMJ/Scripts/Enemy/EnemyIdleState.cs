using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Idle 상태에 들어왔을 때
    public override void Enter()
    {
        // 가만히 있는 상태이기 때문에 Speed를 0으로
        stateMachine.MoveMentSpeedModifier = 0f;
        base.Enter();
        // Animation 전환
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.IdleParameterHash);
    }

    // Idle 상태에서 다른 상태로 전환될 때
    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Enemy.EnemyAnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.Enemy.enemyAI.DetectTargets())
        {
            stateMachine.ChangeState(stateMachine.EnemyChaseState);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossBaseState
{
    public BossChaseState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk 상태로 전환되었을 때
    public override void Enter()
    {
        Debug.Log("Chase State");
        // BaseSpeed에 곱해줄 값 세팅
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.Enter();
        StartAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Boss.bossAI.DetectTargets())
        {
            stateMachine.Boss.bossAI.ChaseTarget();
        }
        else
        {
            stateMachine.Boss.bossAI.target = null;
            stateMachine.ChangeState(BossState.Idle);
        }

        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            stateMachine.ChangeState(BossState.Attack);
        }
    }
}

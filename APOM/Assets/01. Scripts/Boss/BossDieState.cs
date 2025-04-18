using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDieState : BossBaseState
{
    public BossDieState(BossStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void StateEnter()
    {
        // BaseSpeed에 곱해줄 값 세팅
        //stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.BossDie_ParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.BossDie_ParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        //stateMachine.Boss.bossAI.ChaseTarget();

        //if (stateMachine.Boss.bossAI.DetectTargets())
        //{
        //    stateMachine.Boss.bossAI.ChaseTarget();
        //}
        //else
        //{
        //    stateMachine.Boss.bossAI.target = null;
        //    stateMachine.ChangeState(BossState.Idle);
        //}

        //if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        //{
        //    stateMachine.ChangeState(BossState.Attack);
        //}
    }
}

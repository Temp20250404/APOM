using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkState : BossBaseState
{
    public BossWalkState(BossStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void StateEnter()
    {
        // BaseSpeed에 곱해줄 값 세팅
        stateMachine.MoveMentSpeedModifier = groundData.WalkSpeedModifier;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.WalkParameterHash);
        stateMachine.Boss.bossAI.StartWalk();
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.WalkParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (stateMachine.Boss.bossAI.DetectTargets())
        {
            stateMachine.ChangeState(BossState.Chase);
            Debug.Log("Chase State");
            return;
        }

        if (stateMachine.Boss.bossAI.EndWalk())
        {
            stateMachine.ChangeState(BossState.Idle); // 도착 시 Idle 상태로 전환
            return;
        }
    }
}

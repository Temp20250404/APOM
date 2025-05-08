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
        stateMachine.MoveMentSpeedModifier = 0;
        base.StateEnter();
        stateMachine.Boss.Anim.SetTrigger(stateMachine.Boss.BossAnimationData.BossDie_ParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void StateExit()
    {
        base.StateExit();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }
}

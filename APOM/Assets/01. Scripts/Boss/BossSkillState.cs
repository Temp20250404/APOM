using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillState : BossBaseState
{
    public BossSkillState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill1ParameterHash);
    }

    public override void StateExit()
    {
        base.StateExit();

        StopAnimation(stateMachine.Boss.BossAnimationData.BossSkill1ParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
    }

    public override void StateUpdate()
    {
        AnimatorStateInfo animStateInfo = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.normalizedTime >= 1f && animStateInfo.IsTag("Skills"))
        {
            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = stateMachine.Boss.bossID;
            packet.BossState = (int)BossState.Idle;
            Managers.Network.Send(packet);
        }
    }
}

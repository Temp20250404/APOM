using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillState1 : BossBaseState
{
    //public BossSkill bossSkill;

    public BossSkillState1(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill2ParameterHash);
    }

    public override void StateExit()
    {
        base.StateExit();

        StopAnimation(stateMachine.Boss.BossAnimationData.BossSkill2ParameterHash);
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
            packet.CurSpeed = 0f;
            Managers.Network.Send(packet);
            //stateMachine.Boss.bossAI.ClearSkill();

            // 서버에 상태 변경을 요청하는 메서드 호출
        }
    }

    //public void SetSkill(BossSkill skill)
    //{
    //    this.bossSkill = skill;
    //}
}

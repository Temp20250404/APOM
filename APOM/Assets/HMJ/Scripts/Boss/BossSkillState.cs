using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSkillState : BossBaseState
{
    public BossSkill bossSkill;

    public BossSkillState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    public override void Enter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.Enter();
        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
        StartAnimation(bossSkill.animationHash);
    }

    public override void Exit()
    {
        base.Exit();

        StopAnimation(bossSkill.animationHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
    }

    public override void Update()
    {
        AnimatorStateInfo animStateInfo = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.normalizedTime >= 1f && animStateInfo.IsTag("Skills"))
        {
            Debug.Log("[Skill 종료] Idle로 전환합니다.");
            stateMachine.ChangeState(BossState.Idle);
            stateMachine.Boss.bossAI.ClearSkill();
        }
    }

    public void SetSkill(BossSkill skill)
    {
        this.bossSkill = skill;
    }
}

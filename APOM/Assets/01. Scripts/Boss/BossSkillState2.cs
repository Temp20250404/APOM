using UnityEngine;
using Game;

public class BossSkillState2 : BossBaseState
{
    public BossSkillState2(BossStateMachine stateMachine) : base(stateMachine) { }

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();

        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
        StartAnimation(stateMachine.Boss.BossAnimationData.BossSkill3ParameterHash);
    }

    public override void StateUpdate()
    {
        AnimatorStateInfo anim = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
        if (anim.IsTag("Skills") && anim.normalizedTime >= 1f)
        {
            SendBossState(BossState.Idle);
        }
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.BossSkill3ParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.BossSkill_ParameterHash);
    }
}

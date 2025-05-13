using UnityEngine;

public class BossAttackState1 : BossBaseState
{
    public BossAttackState1(BossStateMachine stateMachine) : base(stateMachine) { }

    public override void StateEnter()
    {
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
        StartAnimation(stateMachine.Boss.BossAnimationData.Attack2ParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        AnimatorStateInfo animState = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
        if (animState.IsTag("Attack") && animState.normalizedTime >= 0.8f)
        {
            SendBossState(BossState.Idle);
            return;
        }

        if (!stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            float chaseSpeed = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
            SendBossState(BossState.Chase, chaseSpeed);
        }
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.Attack2ParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
    }
}

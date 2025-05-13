using UnityEngine;

public class BossAttackState : BossBaseState
{
    public BossAttackState(BossStateMachine stateMachine) : base(stateMachine) { }

    public override void StateEnter()
    {
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
        StartAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);
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
        StopAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
    }
}

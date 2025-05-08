using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine) { }

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            AnimatorStateInfo animState = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
            if (animState.IsTag("Idle") && animState.normalizedTime >= 0.8f)
                SendBossState(BossState.Attack);
        }
        else if (stateMachine.Boss.bossAI.DetectTargets(stateMachine.Boss.SOData.PlayerChasingRange))
        {
            float chaseSpeed = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
            SendBossState(BossState.Chase, chaseSpeed);
        }
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);
    }
}

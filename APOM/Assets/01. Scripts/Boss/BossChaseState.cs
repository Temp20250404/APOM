using UnityEngine;

public class BossChaseState : BossBaseState
{
    public BossChaseState(BossStateMachine stateMachine) : base(stateMachine) { }

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (!stateMachine.Boss.bossAI.DetectTargets(stateMachine.Boss.SOData.PlayerChasingRange))
        {
            stateMachine.Boss.bossAI.target = null;
            SendBossState(BossState.Idle);
        }
        else if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            SendBossState(BossState.Attack);
        }
        else
        {
            stateMachine.Boss.bossAI.CSChaseTarget();
        }
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }
}

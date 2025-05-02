using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossBaseState
{
    public BossChaseState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk 상태로 전환되었을 때
    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();


        stateMachine.Boss.bossAI.CSChaseTarget();

        if (!stateMachine.Boss.bossAI.DetectTargets(stateMachine.Boss.SOData.PlayerChasingRange))
        {
            stateMachine.Boss.bossAI.target = null;

            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = stateMachine.Boss.bossID;
            packet.BossState = (int)BossState.Idle;
            Managers.Network.Send(packet);
        }


        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = stateMachine.Boss.bossID;
            packet.BossState = (int)BossState.Attack;
            Managers.Network.Send(packet);
        }
    }
}

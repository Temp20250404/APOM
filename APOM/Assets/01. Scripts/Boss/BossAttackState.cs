using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackState : BossBaseState
{
    public BossAttackState(BossStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void StateEnter()
    {
        // 가만히 있는 상태이기 때문에 Speed를 0으로
        //stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        // Animation 전환
        StartAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
        StartAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);


    }

    // Idle 상태에서 다른 상태로 전환될 때
    public override void StateExit()
    {
        base.StateExit();

        StopAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        AnimatorStateInfo animStateInfo = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsTag("Attack") && animStateInfo.normalizedTime >= 0.8f)
        {
            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = stateMachine.Boss.bossID;
            packet.BossState = (int)BossState.Idle;
            packet.CurSpeed = 0f;
            Managers.Network.Send(packet);
        }

        if (!stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = stateMachine.Boss.bossID;
            packet.BossState = (int)BossState.Chase;
            packet.CurSpeed = stateMachine.Boss.SOData.GroundData.ChasingSpeedModifier *
                stateMachine.Boss.SOData.GroundData.BaseSpeed;
            Managers.Network.Send(packet);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkState : BossBaseState
{
    public BossWalkState(BossStateMachine stateMachine) : base(stateMachine)
    {
    }

    private float lastRotateSendTime = 0f;
    private float rotateCooldown = 0.2f;

    public override void StateEnter()
    {
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.WalkParameterHash);
        //stateMachine.Boss.bossAI.StartWalk();
    }

    // Walk 상태에서 다른 상태로 전환될 때
    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.WalkParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        //  회전 중이 아니고, 아직 타겟을 안 보고 있다면 회전 요청
        if (!stateMachine.Boss.bossAI.IsRotating && !stateMachine.Boss.bossAI.IsLookingAtTarget(5f) && Time.time - lastRotateSendTime > rotateCooldown)
        {
            stateMachine.Boss.bossAI.CSRotateToTarget();
            lastRotateSendTime = Time.time;
        }

        if (stateMachine.Boss.bossAI.IsLookingAtTarget(5f))
        {
            if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
            {
                SendBossState(BossState.Idle);
            }
        }

        //if (stateMachine.Boss.bossAI.DetectTargets(stateMachine.Boss.SOData.PlayerChasingRange))
        //{
        //    stateMachine.ChangeState(BossState.Chase);
        //    Debug.Log("Chase State");
        //    return;
        //}

        //if (stateMachine.Boss.bossAI.EndWalk())
        //{
        //    stateMachine.ChangeState(BossState.Idle); // 도착 시 Idle 상태로 전환
        //    return;
        //}
    }
}

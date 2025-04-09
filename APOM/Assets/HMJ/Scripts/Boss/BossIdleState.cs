using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : BossBaseState
{
    private float idleTime;
    private float waitTime;
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Idle 상태에 들어왔을 때
    public override void Enter()
    {
        // 가만히 있는 상태이기 때문에 Speed를 0으로
        stateMachine.MoveMentSpeedModifier = 0f;
        base.Enter();
        // Animation 전환
        StartAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);

        idleTime = Random.Range(2f, 4f);
        waitTime = 0f;
    }

    // Idle 상태에서 다른 상태로 전환될 때
    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            AnimatorStateInfo animStateInfo = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
            if (animStateInfo.IsTag("Idle") && animStateInfo.normalizedTime >= 0.8f)
            {
                stateMachine.ChangeState(BossState.Attack);
            }
        }

        if (!stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData) && stateMachine.Boss.bossAI.DetectTargets())
        {
            stateMachine.ChangeState(BossState.Chase);
        }

        UpdateWalk();
    }

    private void UpdateWalk()
    {
        waitTime += Time.deltaTime;
        if (waitTime >= idleTime)
        {
            stateMachine.ChangeState(BossState.Walk);
        }
    }
}

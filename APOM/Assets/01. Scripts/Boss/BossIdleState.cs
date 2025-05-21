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
            // 타겟을 안 보고 있으면 회전용 Walk 상태로 전환
            if (!stateMachine.Boss.bossAI.IsLookingAtTarget(5f))
            {
                float walkSpeed = groundData.WalkSpeedModifier * groundData.BaseSpeed;
                SendBossState(BossState.Walk, walkSpeed);
                return;
            }

            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                SendBossState(BossState.Attack);
            }
            else
            {
                SendBossState(BossState.Attack1);
            }
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

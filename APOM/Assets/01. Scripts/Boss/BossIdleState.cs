using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine) { }

    private float attackCooldown = 2.0f; // 공격 쿨타임 (초)
    private float lastAttackTime = -Mathf.Infinity;

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        // 1. 타겟 시야 체크 → 회전 필요 시 Walk 전환
        if (!stateMachine.Boss.bossAI.IsLookingAtTarget(5f))
        {
            float walkSpeed = groundData.WalkSpeedModifier * groundData.BaseSpeed;
            SendBossState(BossState.Walk, walkSpeed);
            return;
        }

        // 2. 공격 범위 안에 있을 때만 공격
        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            // 쿨타임 검사
            if (Time.time - lastAttackTime >= attackCooldown)
            {
                lastAttackTime = Time.time;

                int rand = Random.Range(0, 2);
                SendBossState(rand == 0 ? BossState.Attack : BossState.Attack1);
            }
        }
        // 3. 감지 범위 내에 있고 공격 범위는 아닐 경우 → 추적
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

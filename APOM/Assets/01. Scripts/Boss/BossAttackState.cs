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
        if (animState.IsTag("Attack") && animState.normalizedTime >= 0.9f)
        {
            if (!stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
            {
                float walkSpeed = groundData.WalkSpeedModifier * groundData.BaseSpeed;
                SendBossState(BossState.Walk, walkSpeed);
            }
            else
            {
                Debug.Log(1);
                // 공격 범위에 있을 때는 Idle 상태로 전환
                SendBossState(BossState.Idle);
            }
        }
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
    }
}

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
        // ������ �ִ� �����̱� ������ Speed�� 0����
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        // Animation ��ȯ
        int rand = Random.Range(0, 2);
        if (rand == 0)
        {
            StartAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
            StartAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);
        }
        else
        {
            StartAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
            StartAnimation(stateMachine.Boss.BossAnimationData.Attack2ParameterHash);
        }

    }

    // Idle ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void StateExit()
    {
        base.StateExit();

        StopAnimation(stateMachine.Boss.BossAnimationData.Attack_ParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Boss.BossAnimationData.Attack2ParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        AnimatorStateInfo animStateInfo = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsTag("Attack") && animStateInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(BossState.Idle);
        }

        if (!stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            stateMachine.ChangeState(BossState.Chase);
        }
    }
}

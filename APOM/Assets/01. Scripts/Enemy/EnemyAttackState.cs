using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyBaseState
{
    public EnemyAttackState(EnemyStateMachine stateMachine) : base(stateMachine)
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
            StartAnimation(stateMachine.Enemy.EnemyAnimationData.Attack_ParameterHash);
            StartAnimation(stateMachine.Enemy.EnemyAnimationData.AttackParameterHash);
        }
        else
        {
            StartAnimation(stateMachine.Enemy.EnemyAnimationData.Attack_ParameterHash);
            StartAnimation(stateMachine.Enemy.EnemyAnimationData.Attack2ParameterHash);
        }
       
    }

    // Idle ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void StateExit()
    {
        base.StateExit();

        StopAnimation(stateMachine.Enemy.EnemyAnimationData.Attack_ParameterHash);
        StopAnimation(stateMachine.Enemy.EnemyAnimationData.AttackParameterHash);
        StopAnimation(stateMachine.Enemy.EnemyAnimationData.Attack2ParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        AnimatorStateInfo animStateInfo = stateMachine.Enemy.Anim.GetCurrentAnimatorStateInfo(0);
        if (animStateInfo.IsTag("Attack") && animStateInfo.normalizedTime >= 0.8f)
        {
            stateMachine.ChangeState(EnemyState.Idle);
        }

        if (!stateMachine.Enemy.enemyAI.IsAttackRange(stateMachine.Enemy.SOData))
        {
            stateMachine.ChangeState(EnemyState.Chase);
        }
    }
}

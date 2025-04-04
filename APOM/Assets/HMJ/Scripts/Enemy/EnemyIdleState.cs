using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyBaseState
{
    public EnemyIdleState(EnemyStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Idle ���¿� ������ ��
    public override void Enter()
    {
        // ������ �ִ� �����̱� ������ Speed�� 0����
        stateMachine.MoveMentSpeedModifier = 0f;
        base.Enter();
        // Animation ��ȯ
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.IdleParameterHash);
    }

    // Idle ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void Exit()
    {
        base.Exit();

        StopAnimation(stateMachine.Enemy.EnemyAnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if(stateMachine.Enemy.enemyAI.DetectTargets())
        {
            stateMachine.ChangeState(stateMachine.EnemyChaseState);
        }
    }
}
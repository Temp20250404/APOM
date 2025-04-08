using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalkState : EnemyBaseState
{
    public EnemyWalkState(EnemyStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // BaseSpeed�� ������ �� ����
        stateMachine.MoveMentSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Enemy.EnemyAnimationData.WalkParameterHash);
        stateMachine.Enemy.enemyAI.StartWalk();
    }

    // Walk ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Enemy.EnemyAnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        if(stateMachine.Enemy.enemyAI.DetectTargets())
        {
            stateMachine.ChangeState(EnemyState.Chase);
            Debug.Log("Chase State");
            return;
        }

        if (stateMachine.Enemy.enemyAI.EndWalk())
        {
            stateMachine.ChangeState(EnemyState.Idle); // ���� �� Idle ���·� ��ȯ
            return;
        }
    }
}

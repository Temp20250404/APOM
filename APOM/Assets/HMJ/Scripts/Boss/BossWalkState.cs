using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkState : BossBaseState
{
    public BossWalkState(BossStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        // BaseSpeed�� ������ �� ����
        stateMachine.MoveMentSpeedModifier = groundData.WalkSpeedModifier;
        base.Enter();
        StartAnimation(stateMachine.Boss.BossAnimationData.WalkParameterHash);
        stateMachine.Boss.bossAI.StartWalk();
    }

    // Walk ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Boss.BossAnimationData.WalkParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Boss.bossAI.DetectTargets())
        {
            stateMachine.ChangeState(BossState.Chase);
            Debug.Log("Chase State");
            return;
        }

        if (stateMachine.Boss.bossAI.EndWalk())
        {
            stateMachine.ChangeState(BossState.Idle); // ���� �� Idle ���·� ��ȯ
            return;
        }
    }
}

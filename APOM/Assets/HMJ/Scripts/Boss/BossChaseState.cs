using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChaseState : BossBaseState
{
    public BossChaseState(BossStateMachine stateMachine) : base(stateMachine)
    {

    }

    // Walk ���·� ��ȯ�Ǿ��� ��
    public override void Enter()
    {
        Debug.Log("Chase State");
        // BaseSpeed�� ������ �� ����
        stateMachine.MoveMentSpeedModifier = groundData.ChasingSpeedModifier * groundData.BaseSpeed;
        base.Enter();
        StartAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    // Walk ���¿��� �ٸ� ���·� ��ȯ�� ��
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Boss.BossAnimationData.ChasingParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Boss.bossAI.DetectTargets())
        {
            stateMachine.Boss.bossAI.ChaseTarget();
        }
        else
        {
            stateMachine.Boss.bossAI.target = null;
            stateMachine.ChangeState(BossState.Idle);
        }

        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            stateMachine.ChangeState(BossState.Attack);
        }
    }
}

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

    // Idle ���¿� ������ ��
    public override void Enter()
    {
        // ������ �ִ� �����̱� ������ Speed�� 0����
        stateMachine.MoveMentSpeedModifier = 0f;
        base.Enter();
        // Animation ��ȯ
        StartAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);

        idleTime = Random.Range(2f, 4f);
        waitTime = 0f;
    }

    // Idle ���¿��� �ٸ� ���·� ��ȯ�� ��
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

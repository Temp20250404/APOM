using UnityEngine;

public class BossIdleState : BossBaseState
{
    public BossIdleState(BossStateMachine stateMachine) : base(stateMachine) { }

    private float lastRotateSendTime = 0f;
    private float rotateCooldown = 0.2f;

    public override void StateEnter()
    {
        stateMachine.MoveMentSpeedModifier = 0f;
        base.StateEnter();
        StartAnimation(stateMachine.Boss.BossAnimationData.IdleParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        //  회전 중이 아니고, 아직 타겟을 안 보고 있다면 회전 요청
        if (!stateMachine.Boss.bossAI.IsRotating && !stateMachine.Boss.bossAI.IsLookingAtTarget(5f) && Time.time - lastRotateSendTime > rotateCooldown)
        {
            stateMachine.Boss.bossAI.CSRotateToTarget();
            lastRotateSendTime = Time.time;
        }

        if (stateMachine.Boss.bossAI.IsAttackRange(stateMachine.Boss.SOData))
        {
            AnimatorStateInfo animState = stateMachine.Boss.Anim.GetCurrentAnimatorStateInfo(0);
            if (animState.IsTag("Idle") && animState.normalizedTime >= 0.8f)
                SendBossState(BossState.Attack);
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

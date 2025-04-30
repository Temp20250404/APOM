using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAttackState : PlayerGroundState
{
    protected Quaternion motionRotate;

    protected GameObject target;

    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        stateMachine.movementSpeedModifier = 0;

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.attackParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        Rotate(GetCameraDirection());
        //var atttackAnimInfo = stateMachine.player.animator.GetCurrentAnimatorStateInfo(0);

        //if (atttackAnimInfo.normalizedTime >= 1f)
        //{
        //    stateMachine.ChangeState(stateMachine.idleState);
        //}
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.attackParameterHash);
    }

    public override void StateHandleInput()
    {
        base.StateHandleInput();
    }

    public override void StatePhysicsUpdate()
    {
        base.StatePhysicsUpdate();
    }
}

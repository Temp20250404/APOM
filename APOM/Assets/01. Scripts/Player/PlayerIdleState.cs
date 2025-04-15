using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerDefaultState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        Debug.Log("Idle State");
        stateMachine.movementSpeedModifier = 0f;
        stateMachine.movementInput = Vector2.zero;

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.idleParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        if (stateMachine.movementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.moveState);
            return;
        }

    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.idleParameterHash);
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

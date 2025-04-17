using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerDefaultState
{
    public PlayerMoveState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : Move State");
        stateMachine.movementSpeedModifier = defaultData.moveSpeedModifier;

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.moveParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
        CheckMove();
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.moveParameterHash);
    }

    public override void StateHandleInput()
    {
        base.StateHandleInput();
    }

    public override void StatePhysicsUpdate()
    {
        base.StatePhysicsUpdate();
    }

    private void CheckMove()
    {
        if (!stateMachine.player.inputController.isMoving)
        {
            //stateMachine.ChangeState(stateMachine.idleState);
        }
    }
}
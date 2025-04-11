using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultState : PlayerBaseState
{
    public PlayerDefaultState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.defaultParameterHash);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        Move();
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.defaultParameterHash);
    }

    public override void StateHandleInput()
    {
        base.StateHandleInput();
    }

    public override void StatePhysicsUpdate()
    {
        base.StatePhysicsUpdate();
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext context)
    {
        //if (stateMachine.movementInput == Vector2.zero)
        //{
        //    return;
        //}
        stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.W] = false;
        stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.S] = false;
        stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.A] = false;
        stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.D] = false;
        stateMachine.movementInput = Vector2.zero;

        stateMachine.ChangeState(stateMachine.idleState);

        base.OnMoveCanceled(context);
    }
}

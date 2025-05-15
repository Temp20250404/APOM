using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultState : PlayerGroundState
{
    private float holdTime = 0f;
    private float holdTimeLimit = 0.2f;
    private bool isHold = false;

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

        CheckAttack();
        DodgeHoldCheck();
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

        stateMachine.ChangeState(stateMachine.idleState);

        base.OnMoveCanceled(context);
    }

    private void CheckAttack()
    {
        isHold = stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.RCLICK];
        if (GetInputNoramlAttack() == true)
        {
            stateMachine.ChangeState(stateMachine.normalAttackState);
        }
        else if (stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.NUM1] == true)
        {
            stateMachine.ChangeState(stateMachine.rainArrowState);
        }
        else if (stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.NUM2] == true)
        {
            stateMachine.ChangeState(stateMachine.poisonArrowState);
        }
        else if (stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.NUM3] == true)
        {
            stateMachine.ChangeState(stateMachine.powerShotState);
        }
        else if (stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.NUM4] == true)
        {
            stateMachine.ChangeState(stateMachine.backStepShotState);
        }
        else if (stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.NUM5] == true)
        {
            stateMachine.ChangeState(stateMachine.rapidFireState);
        }
    }

    private void DodgeHoldCheck()
    {
        if (isHold == false)
        {
            holdTime = 0f;
            return;
        }

        holdTime += Time.deltaTime;
        if (holdTime >= holdTimeLimit)
        {
            stateMachine.ChangeState(stateMachine.dodgeState);
            holdTime = 0f;
        }
    }
}

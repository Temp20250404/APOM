using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultState : PlayerGroundState
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

        CheckAttack();
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
        if (GetInputNoramlAttack() == true)
        {
            stateMachine.ChangeState(stateMachine.normalAttackState);
        }
        else if (stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.RCLICK] == true)
        {
            stateMachine.ChangeState(stateMachine.dodgeState);
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
}

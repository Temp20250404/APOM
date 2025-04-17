using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDefaultState : PlayerBaseState
{
    private Coroutine delayedIdleCoroutine = null;

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

    protected override void OnMovePerformed(InputAction.CallbackContext context)
    {
        base.OnMovePerformed(context);

        if (!stateMachine.player.inputController.isMainPlayer && delayedIdleCoroutine != null)
        {
            StopDelayedChangeIdleState();
        }
    }

    protected override void OnMoveCanceled(InputAction.CallbackContext context)
    {
        //if (stateMachine.movementInput == Vector2.zero)
        //{
        //    return;
        //}
        if (stateMachine.player.inputController.isMainPlayer)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
        else
        {
            StartDelayedChangeIdleState();
        }

        base.OnMoveCanceled(context);
    }

    private void StartDelayedChangeIdleState()
    {
        if (delayedIdleCoroutine != null) 
            return;

        delayedIdleCoroutine = stateMachine.player.inputController.StartCoroutine(CDelayedChangeIdleState());
    }

    private void StopDelayedChangeIdleState()
    {
        if (delayedIdleCoroutine != null)
        {
            stateMachine.player.inputController.StopCoroutine(delayedIdleCoroutine);
            delayedIdleCoroutine = null;
        }
    }

    private IEnumerator CDelayedChangeIdleState()
    {
        yield return new WaitForSeconds(0.5f);

        stateMachine.ChangeState(stateMachine.idleState);
        delayedIdleCoroutine = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerDefaultState
{
    private Coroutine delayedIdleCoroutine = null;

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

    protected override void OnMovePerformed(InputAction.CallbackContext context)
    {
        base.OnMovePerformed(context);

        if (!stateMachine.player.inputController.isMainPlayer && delayedIdleCoroutine != null)
        {
            StopDelayedChangeIdleState();
        }
    }

    private void CheckMove()
    {
        if (!stateMachine.player.inputController.isMoving)
        {
            if (stateMachine.player.inputController.isMainPlayer)
            {
                stateMachine.ChangeState(stateMachine.idleState);
            }
            else
            {
                StartDelayedChangeIdleState();
            }
        }
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
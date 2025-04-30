using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDodgeState : PlayerAttackState
{
    private Vector3 dodgeDir;

    public PlayerDodgeState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();

        stateMachine.movementSpeedModifier = defaultData.moveSpeedModifier * 0.8f;

        StartAnimation(stateMachine.player.animationData.dodgeParameterHash);

        if (GetMovementDirection() == Vector3.zero)
        {
            dodgeDir = stateMachine.player.transform.forward;
        }
        else
        {
            dodgeDir = GetMovementDirection();
        }

        Rotate(dodgeDir);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        Move(dodgeDir);
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.dodgeParameterHash);
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
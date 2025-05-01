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

        Vector3 inputDir = GetMovementDirection();

        if (inputDir.sqrMagnitude < 0.01f)
        {
            dodgeDir = Quaternion.Euler(0f, stateMachine.player.inputController.recivePacketRotation, 0f) * Vector3.forward;
        }
        else
        {
            dodgeDir = new Vector3(GetMovementDirection().x, 0f, GetMovementDirection().z).normalized;
        }
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
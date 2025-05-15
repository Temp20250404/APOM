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

        stateMachine.player.Stat.isDodge = true;

        stateMachine.movementSpeedModifier = defaultData.moveSpeedModifier * 0.8f;

        StartAnimation(stateMachine.player.animationData.dodgeParameterHash);

        Vector3 inputDir = GetMovementDirection(stateMachine.player.inputController.wasdDir);
        inputDir.y = 0f;

        float yaw;

        if (inputDir.sqrMagnitude < 0.01f)
        {
            yaw = stateMachine.player.inputController.recivePacketRotation;
        }
        else
        {
            yaw = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg;
        }

        dodgeDir = Quaternion.Euler(0f, yaw, 0f) * Vector3.forward;

        DirectRotate((Quaternion.Euler(0f, 90f, 0f) * dodgeDir));
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        Move(dodgeDir);
    }

    public override void StateExit()
    {
        base.StateExit();
        stateMachine.player.Stat.isDodge = false;
        StopAnimation(stateMachine.player.animationData.dodgeParameterHash);
        DirectRotate((Quaternion.Euler(0f, 0f, 0f) * dodgeDir));
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
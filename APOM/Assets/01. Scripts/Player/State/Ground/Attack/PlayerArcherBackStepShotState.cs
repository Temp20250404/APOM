using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcherBackStepShotState : PlayerAttackState
{
    private int skillindex = 5;
    private Vector3 backStepDir;

    public PlayerArcherBackStepShotState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();

        stateMachine.movementSpeedModifier = defaultData.moveSpeedModifier * 1.2f;

        StartAnimation(stateMachine.player.animationData.skill4ParameterHash);

        backStepDir = Quaternion.Euler(0f, stateMachine.player.inputController.recivePacketRotation, 0f) * Vector3.back;

        SendAttackPacket(skillindex);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        Move(backStepDir);
    }

    public override void StateExit()
    {
        StopAnimation(stateMachine.player.animationData.skill4ParameterHash);
        base.StateExit();
        skillManager.StartCooldown(skillindex);
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

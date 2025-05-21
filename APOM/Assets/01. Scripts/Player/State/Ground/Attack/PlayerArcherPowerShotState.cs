using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcherPowerShotState : PlayerAttackState
{
    private int skillindex = 4;

    public PlayerArcherPowerShotState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.skill3ParameterHash);

        SendAttackPacket(skillindex);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit()
    {
        StopAnimation(stateMachine.player.animationData.skill3ParameterHash);

        //DirectRotate(GetCameraDirection());
        base.StateExit();
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
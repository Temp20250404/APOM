using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcherPoisonArrowState : PlayerAttackState
{
    private int skillindex = 3;

    public PlayerArcherPoisonArrowState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.skill2ParameterHash);

        SendAttackPacket(skillindex);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit()
    {
        StopAnimation(stateMachine.player.animationData.skill2ParameterHash);

        //DirectRotate(GetCameraDirection());
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

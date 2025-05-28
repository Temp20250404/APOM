using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArcherRainArrowState : PlayerAttackState
{
    private int skillindex = 2;

    public PlayerArcherRainArrowState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }
    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.skill1ParameterHash);

        //DirectRotate((Quaternion.Euler(0f, 90f, 0f) * GetCameraDirection()));
    }

    public override void StateUpdate()
    {
        base.StateUpdate();
    }

    public override void StateExit()
    {
        StopAnimation(stateMachine.player.animationData.skill1ParameterHash);

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

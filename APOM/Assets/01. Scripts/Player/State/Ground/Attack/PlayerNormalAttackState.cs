using APOM_Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerNormalAttackState : PlayerAttackState
{
    private int skillindex = 0;

    public PlayerNormalAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.normalAttackParameterHash);

        SendAttackPacket(skillindex);
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        
    }

    public override void StateExit()
    {
        StopAnimation(stateMachine.player.animationData.normalAttackParameterHash);

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

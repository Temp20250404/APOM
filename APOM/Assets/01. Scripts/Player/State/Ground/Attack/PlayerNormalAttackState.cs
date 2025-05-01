using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNormalAttackState : PlayerAttackState
{
    public PlayerNormalAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        Debug.Log($"ID : {stateMachine.player.playerID} : NormalAttack State");

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.normalAttackParameterHash);

        DirectRotate(Quaternion.Euler(0f, 41f, 0f) * GetCameraDirection());

        if (targetObject != null)
        {
            Util.SendPacket<CS_PLAYER_ATTACK>(packet =>
            {
                if (targetObject.TryGetComponent<Boss>(out Boss boss))
                {
                    packet.AiID = boss.bossID;
                }
                packet.AttackDamage = stateMachine.player.Stat.CulSkillDamage(1f);
            });
        }
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.normalAttackParameterHash);

        //DirectRotate(GetCameraDirection());
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

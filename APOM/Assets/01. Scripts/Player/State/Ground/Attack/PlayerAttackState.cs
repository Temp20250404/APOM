using APOM_Data;
using Game;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAttackState : PlayerGroundState
{
    protected Quaternion motionRotate;

    public PlayerAttackState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void StateEnter()
    {
        stateMachine.movementSpeedModifier = 0;

        base.StateEnter();
        StartAnimation(stateMachine.player.animationData.attackParameterHash);
        DirectRotate(GetCameraDirection());

        AttackRay();
    }

    public override void StateUpdate()
    {
        base.StateUpdate();

        //var atttackAnimInfo = stateMachine.player.animator.GetCurrentAnimatorStateInfo(0);

        //if (atttackAnimInfo.normalizedTime >= 1f)
        //{
        //    stateMachine.ChangeState(stateMachine.idleState);
        //}
    }

    public override void StateExit()
    {
        base.StateExit();
        StopAnimation(stateMachine.player.animationData.attackParameterHash);

        stateMachine.player.targetObject = null;
        stateMachine.player.targetPosition = Vector3.zero;
    }

    public override void StateHandleInput()
    {
        base.StateHandleInput();
    }

    public override void StatePhysicsUpdate()
    {
        base.StatePhysicsUpdate();
    }

    private void AttackRay()
    {
        float rayDistance = stateMachine.player.Stat.rayRange;

        int enemyMask = (1 << LayerMask.NameToLayer("Enemy")) | (1 << LayerMask.NameToLayer("Boss"));
        int ObstacleMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));

        Ray ray = stateMachine.player.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, enemyMask))
        {
            stateMachine.player.targetObject = hit.collider.gameObject;
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, ObstacleMask))
        {
            stateMachine.player.targetPosition = hit.point;
        }
        else
        {
            stateMachine.player.targetPosition = ray.origin + ray.direction * rayDistance;
        }
    }

    protected void SendAttackPacket(int _skillIndex)
    {
        if (stateMachine.player.targetObject != null)
        {
            Util.SendPacket<CS_PLAYER_ATTACK>(packet =>
            {
                Boss boss = stateMachine.player.targetObject.GetComponentInParent<Boss>();

                if (boss != null)
                {
                    packet.AiID = boss.bossID;
                }

                packet.AttackDamage = stateMachine.player.Stat.CulSkillDamage(skillManager.equippedSkills[_skillIndex].skilloperation);
            });
        }
    }
}

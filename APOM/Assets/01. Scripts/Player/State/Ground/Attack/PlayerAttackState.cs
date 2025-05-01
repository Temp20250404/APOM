using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAttackState : PlayerGroundState
{
    protected Quaternion motionRotate;

    protected GameObject targetObject = null;
    protected Vector3 targetPosition = Vector3.zero;

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

        targetObject = null;
        targetPosition = Vector3.zero;
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

        int enemyMask = 1 << LayerMask.NameToLayer("Enemy");
        int ObstacleMask = (1 << LayerMask.NameToLayer("Ground")) | (1 << LayerMask.NameToLayer("Obstacle"));

        Ray ray = stateMachine.player.mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance, enemyMask))
        {
            targetObject = hit.collider.gameObject;
        }
        else if (Physics.Raycast(ray, out hit, rayDistance, ObstacleMask))
        {
            targetPosition = hit.point;
        }
        else
        {
            targetPosition = ray.origin + ray.direction * rayDistance;
        }
    }
}

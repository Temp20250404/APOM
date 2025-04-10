using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;
    protected readonly EnemyGroundData groundData;


    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Enemy.SOData.GroundData;
    }
    public virtual void StateEnter()
    {
        stateMachine.Enemy.enemyAI.MoveSpeed(stateMachine.MoveMentSpeedModifier);
    }

    public virtual void StateExit()
    {
    }

    public virtual void StateUpdate()
    {

    }

    protected void StartAnimation(int parameterHash)
    {
        stateMachine.Enemy.Anim.SetBool(parameterHash, true);
    }

    protected void StopAnimation(int parameterHash)
    {
        stateMachine.Enemy.Anim.SetBool(parameterHash, false);
    }

    public void StateHandleInput() { }

    public void StatePhysicsUpdate(){ }
}
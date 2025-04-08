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
    public virtual void Enter()
    {
        stateMachine.Enemy.enemyAI.MoveSpeed(stateMachine.MoveMentSpeedModifier);
    }

    public virtual void Exit()
    {
    }

    public virtual void Update()
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
}
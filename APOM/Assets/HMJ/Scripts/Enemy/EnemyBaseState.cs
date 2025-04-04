using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseState : IState
{
    protected EnemyStateMachine stateMachine;


    public EnemyBaseState(EnemyStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
    public virtual void Enter()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Exit()
    {
        throw new System.NotImplementedException();
    }

    public virtual void HandleInput()
    {
        throw new System.NotImplementedException();
    }

    public virtual void PhysicsUpdate()
    {
        throw new System.NotImplementedException();
    }

    public virtual void Update()
    {
        throw new System.NotImplementedException();
    }
}

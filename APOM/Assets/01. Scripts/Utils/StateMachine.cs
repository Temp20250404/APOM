using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    public void StateEnter();
    public void StateUpdate();
    public void StateExit();
    public void StateHandleInput();
    public void StatePhysicsUpdate();
}

public abstract class StateMachine
{
    protected IState currentState;

    public void ChangeState(IState state)
    {
        currentState?.StateExit();
        currentState = state;
        currentState?.StateEnter();
    }

    public void StateUpdate()
    {
        currentState?.StateUpdate();
    }

    public void StateHandleInput()
    {
        currentState?.StateHandleInput();
    }

    public void StatePhysicsUpdate()
    {
        currentState?.StatePhysicsUpdate();
    }
}

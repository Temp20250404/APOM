using System.Collections.Generic;

public interface IState
{
    public void Enter();
    public void Exit();
    public void HandleInput();
    public void Update();
    public void PhysicsUpdate();
}

public enum EnemyState
{
    Idle,
    Walk,
    Chase,
    Attack
}

public abstract class StateMachine
{
    protected IState currentState;

    public Dictionary<EnemyState, IState> states;

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = states[newState];
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()  // 생명주기 아님, Monobe가 없음
    {
        currentState?.Update();
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}

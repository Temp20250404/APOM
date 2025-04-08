using System.Collections.Generic;

public interface IState
{
    public void Enter();
    public void Exit();
    public void Update();
}

public enum EnemyState
{
    Idle,
    Walk,
    Chase,
    Attack
}

public enum BossState
{
    Idle,
    Walk,
    Chase,
    Attack
}

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3,
    Phase4
}

public abstract class StateMachine
{
    protected IState currentState;

    public Dictionary<EnemyState, IState> enemystates;
    public Dictionary<BossState, IState>  bossStates;

    public void ChangeState(EnemyState newState)
    {
        currentState?.Exit();
        currentState = enemystates[newState];
        currentState?.Enter();
    }

    public void ChangeState(BossState newState)
    {
        currentState?.Exit();
        currentState = bossStates[newState];
        currentState?.Enter();
    }

    public void Update()  // 생명주기 아님, Monobe가 없음
    {
        currentState?.Update();
    }
}

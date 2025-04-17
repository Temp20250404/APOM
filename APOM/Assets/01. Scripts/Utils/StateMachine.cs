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

public enum EnemyState
{
    Idle,
    Walk,
    Chase,
    Attack
}

public enum BossState
{
    Idle = Game.BOSS_STATE.Idle,
    Walk = Game.BOSS_STATE.Walk,
    Chase = Game.BOSS_STATE.Chase,
    Attack = Game.BOSS_STATE.Attack,
    Skill1 = Game.BOSS_STATE.Skill1,
    Skill2 = Game.BOSS_STATE.Skill2,
    Skill3 = Game.BOSS_STATE.Skill3,
    Die = Game.BOSS_STATE.Die,
}

public enum BossPhase
{
    Phase1,
    Phase2,
    Phase3,
    Phase4
}

public enum BossSkillType
{
    None = -1,
    Skill1,
    Skill2,
    Skill3
}

public abstract class StateMachine
{
    protected IState currentState;

    public Dictionary<EnemyState, IState> enemystates;
    public Dictionary<BossState, IState>  bossStates;
    public void ChangeState(IState state)
    {
        currentState?.StateExit();
        currentState = state;
        currentState?.StateEnter();
    }

    public void StateUpdate() => currentState?.StateUpdate();

    public void StateHandleInput() => currentState?.StateHandleInput();

    public void StatePhysicsUpdate() => currentState?.StatePhysicsUpdate();


    public void ChangeState(EnemyState newState)
    {
        currentState?.StateExit();
        currentState = enemystates[newState];
        currentState?.StateEnter();
    }

    public void ChangeState(BossState newState)
    {
        currentState?.StateExit();
        currentState = bossStates[newState];
        currentState?.StateEnter();
    }
}

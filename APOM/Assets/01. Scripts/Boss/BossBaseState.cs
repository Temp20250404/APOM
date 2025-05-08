using UnityEngine;
using Game;

public abstract class BossBaseState : IState
{
    protected BossStateMachine stateMachine;
    protected readonly BossGroundData groundData;

    public BossBaseState(BossStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Boss.SOData.GroundData;
    }

    public virtual void StateEnter() { }

    public virtual void StateUpdate() { }

    public virtual void StateExit() { }

    public virtual void StateHandleInput() { }

    public virtual void StatePhysicsUpdate() { }

    protected void StartAnimation(int parameterHash)
    {
        stateMachine.Boss.Anim.SetBool(parameterHash, true);
    }

    protected void StopAnimation(int parameterHash)
    {
        stateMachine.Boss.Anim.SetBool(parameterHash, false);
    }

    protected void SendBossState(BossState nextState, float speed = 0f)
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE
        {
            BossID = stateMachine.Boss.bossID,
            BossState = (uint)nextState,
            CurSpeed = speed
        };
        Managers.Network.Send(packet);
    }
}

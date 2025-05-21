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
        if (!Boss.IsMainClient)
            return;

        CS_MONSTER_AI packet = new CS_MONSTER_AI();
        packet.AiID = stateMachine.Boss.bossID;
        packet.BossState = (uint)nextState;
        packet.CurSpeed = speed;
        packet.BossPos = new Position
        {
            PosX = stateMachine.Boss.transform.position.x,
            PosY = stateMachine.Boss.transform.position.y,
            PosZ = stateMachine.Boss.transform.position.z
        };

        if (nextState != BossState.Chase && nextState != BossState.Walk)
        {
            packet.TargetMovementPos = packet.BossPos;
        }
        else
        {
            packet.TargetMovementPos = new Position
            {
                PosX = stateMachine.Boss.bossAI.target.position.x,
                PosY = stateMachine.Boss.bossAI.target.position.y,
                PosZ = stateMachine.Boss.bossAI.target.position.z
            };
        }
        Managers.Network.Send(packet);
    }
}

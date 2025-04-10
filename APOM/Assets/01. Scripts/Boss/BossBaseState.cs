using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBaseState : IState
{
    protected BossStateMachine stateMachine;
    protected readonly BossGroundData groundData;


    public BossBaseState(BossStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        groundData = stateMachine.Boss.SOData.GroundData;
    }

    public virtual void StateEnter()
    {
        stateMachine.Boss.bossAI.MoveSpeed(stateMachine.MoveMentSpeedModifier);
    }

    public virtual void StateUpdate()
    {
        stateMachine.Boss.bossAI.HandleSkills();

        if (stateMachine.Boss.bossAI.NextSkillToUse == null) return;

        BossSkill skill = stateMachine.Boss.bossAI.NextSkillToUse;

        if (skill != null)
        {
            Debug.Log("[Skill 시작] Skill로 전환합니다.");
            stateMachine.BossSkillState.SetSkill(skill);
            stateMachine.ChangeState(BossState.Skill);
        }
    }

    public virtual void StateExit()
    {
    }

    public virtual void StateHandleInput()
    {
    }

    public virtual void StatePhysicsUpdate()
    {
    }

    protected void StartAnimation(int parameterHash)
    {
        stateMachine.Boss.Anim.SetBool(parameterHash, true);
    }

    protected void StopAnimation(int parameterHash)
    {
        stateMachine.Boss.Anim.SetBool(parameterHash, false);
    }
}

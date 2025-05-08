using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStateMachine : StateMachine
{
    public Boss Boss { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MoveMentSpeedModifier { get; set; }

    public BossIdleState BossIdleState { get; }
    public BossAttackState BossAttackState { get; }
    public BossAttackState1 BossAttackState1 { get; }
    public BossChaseState BossChaseState { get; }
    public BossWalkState BossWalkState { get; }
    public BossSkillState BossSkillState { get; }
    public BossSkillState1 BossSkillState1 { get; }
    public BossSkillState2 BossSkillState2 { get; }
    public BossDieState BossDieState { get; }

    public BossStateMachine(Boss Boss)
    {
        this.Boss = Boss;

        BossIdleState = new BossIdleState(this);
        BossAttackState = new BossAttackState(this);
        BossAttackState1 = new BossAttackState1(this);
        BossChaseState = new BossChaseState(this);
        BossWalkState = new BossWalkState(this);
        BossSkillState = new BossSkillState(this);
        BossSkillState1 = new BossSkillState1(this);
        BossSkillState2 = new BossSkillState2(this);
        BossDieState = new BossDieState(this);

        bossStates = new Dictionary<BossState, IState>
        {
            { BossState.Idle, BossIdleState },
            { BossState.Attack, BossAttackState },
            { BossState.Attack1, BossAttackState1 },
            { BossState.Chase, BossChaseState },
            { BossState.Walk, BossWalkState },
            { BossState.Skill1, BossSkillState },
            { BossState.Skill2, BossSkillState1 },
            { BossState.Skill3, BossSkillState2 },
            { BossState.Die, BossDieState }
        };
    }
}

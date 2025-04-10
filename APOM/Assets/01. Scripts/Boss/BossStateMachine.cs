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
    public BossChaseState BossChaseState { get; }
    public BossWalkState BossWalkState { get; }
    public BossSkillState BossSkillState { get; }

    public BossStateMachine(Boss Boss)
    {
        this.Boss = Boss;

        BossIdleState = new BossIdleState(this);
        BossAttackState = new BossAttackState(this);
        BossChaseState = new BossChaseState(this);
        BossWalkState = new BossWalkState(this);
        BossSkillState = new BossSkillState(this);

        bossStates = new Dictionary<BossState, IState>
        {
            { BossState.Idle, BossIdleState },
            { BossState.Attack, BossAttackState },
            { BossState.Chase, BossChaseState },
            { BossState.Walk, BossWalkState },
            { BossState.Skill, BossSkillState }
        };
    }
}

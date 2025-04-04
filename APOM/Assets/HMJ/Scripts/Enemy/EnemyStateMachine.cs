using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : StateMachine
{
    public Enemy Enemy { get; }

    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MoveMentSpeedModifier { get; set; }

    public EnemyIdleState EnemyIdleState { get; }
    public EnemyAttackState EnemyAttackState { get; }
    public EnemyChaseState EnemyChaseState { get; }

    public EnemyStateMachine(Enemy enemy)
    {
        this.Enemy = enemy;
        //Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();

        //EnemyIdleState = new EnemyIdleState(this);
        //EnemyAttackState = new EnemyAttackState(this);
        //EnemyChaseState = new EnemyChaseState(this);

        //MovementSpeed = enemy.SOData.GroundData.BaseSpeed;
        //RotationDamping = enemy.SOData.GroundData.BaseRotationDamping;
    }
}

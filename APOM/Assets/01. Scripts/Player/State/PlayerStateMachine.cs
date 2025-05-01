using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player player { get; }

    public PlayerIdleState idleState { get; }
    public PlayerMoveState moveState { get; }

    public PlayerNormalAttackState normalAttackState { get; }
    public PlayerDodgeState dodgeState { get; }
    public PlayerArcherRainArrowState rainArrowState { get; }
    public PlayerArcherPoisonArrowState poisonArrowState { get; }
    public PlayerArcherPowerShotState powerShotState { get; }
    public PlayerArcherBackStepShotState backStepShotState { get; }
    public PlayerArcherRapidFireState rapidFireState { get; }

    public Vector2 movementInput { get; set; }
    public float movementSpeed { get; private set; }
    public float rotationDamping { get; private set; }
    public float movementSpeedModifier { get; set; } = 1f;

    public PlayerStateMachine(Player player)
    {
        this.player = player;

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);

        normalAttackState = new PlayerNormalAttackState(this);
        dodgeState = new PlayerDodgeState(this);
        rainArrowState = new PlayerArcherRainArrowState(this);
        poisonArrowState = new PlayerArcherPoisonArrowState(this);
        powerShotState = new PlayerArcherPowerShotState(this);
        backStepShotState = new PlayerArcherBackStepShotState(this);
        rapidFireState = new PlayerArcherRapidFireState(this);

        movementSpeed = player.Stat.moveSpeed;
        rotationDamping = player.data.defaultData.baseRotationDamping;
    }
}

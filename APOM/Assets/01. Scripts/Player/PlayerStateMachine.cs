using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    public Player player { get; }

    public PlayerIdleState idleState { get; }
    public PlayerMoveState moveState { get; }

    public Vector2 movementInput { get; set; }
    public float movementSpeed { get; private set; }
    public float rotationDamping { get; private set; }
    public float movementSpeedModifier { get; set; } = 1f;

    public PlayerStateMachine(Player player)
    {
        this.player = player;

        idleState = new PlayerIdleState(this);
        moveState = new PlayerMoveState(this);

        movementSpeed = player.data.defaultData.baseSpeed;
        rotationDamping = player.data.defaultData.baseRotationDamping;
    }
}

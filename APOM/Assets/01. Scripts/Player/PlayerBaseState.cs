using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static PlayerController;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerDefaultData defaultData;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
        defaultData = this.stateMachine.player.data.defaultData;
    }

    public virtual void StateEnter()
    {
        AddInputActionsCallbacks();
    }

    public virtual void StateUpdate()
    {

    }

    public virtual void StateExit()
    {
        RemoveInputActionsCallbacks();
    }

    public virtual void StateHandleInput()
    {
        ReadMovementInput();
    }

    public virtual void StatePhysicsUpdate()
    {
    }

    protected void StartAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.player.animator.SetBool(animationHash, false);
    }

    private void ReadMovementInput()
    {
        stateMachine.movementInput = stateMachine.player.inputController.playerActions.Move.ReadValue<Vector2>();
    }

    protected virtual void Move()
    {
        Vector3 movementDirection = GetMovementDirection();

        Move(movementDirection);
        Rotate(movementDirection);
    }

    protected void Move(Vector3 direction)
    {
        float movementSpeed = GetMovementSpeed();

        stateMachine.player.characterController.Move((direction * movementSpeed) * Time.deltaTime);
    }

    protected Vector3 GetMovementDirection()
    {
        float radian = stateMachine.player.inputController.recivePacketRotation * Mathf.Deg2Rad;

        Vector3 forward = new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
        Vector3 right = new Vector3(Mathf.Cos(radian), 0f, -Mathf.Sin(radian));

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * GetInputWASD().y + right * GetInputWASD().x;
    }

    private float GetMovementSpeed()
    {
        float moveSpeed = stateMachine.movementSpeed * stateMachine.movementSpeedModifier;
        return moveSpeed;
    }

    protected void Rotate(Vector3 direction)
    {
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = Quaternion.Slerp(playerTransform.rotation, targetRotation, stateMachine.rotationDamping * Time.deltaTime);
        }
    }

    protected Vector2 GetInputWASD()
    {
        bool[] inputWASD = stateMachine.player.inputController.recivePacketWASD;
        Vector2 input = Vector2.zero;

        if (inputWASD[(int)EKEYINPUT.W])
        {
            input.y += 1f; // W
        }
        if (inputWASD[(int)EKEYINPUT.S])
        {
            input.y -= 1f; // S
        }
        if (inputWASD[(int)EKEYINPUT.A])
        {
            input.x -= 1f; // A
        }
        if (inputWASD[(int)EKEYINPUT.D])
        {
            input.x += 1f; // D
        }
        input = input.normalized;

        return input;
    }

    protected virtual void AddInputActionsCallbacks()
    {
        PlayerController input = stateMachine.player.inputController;
        input.playerActions.Move.canceled += OnMoveCanceled;
    }

    protected virtual void RemoveInputActionsCallbacks()
    {
        PlayerController input = stateMachine.player.inputController;
        input.playerActions.Move.canceled -= OnMoveCanceled;
    }

    protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
    {
    }
}

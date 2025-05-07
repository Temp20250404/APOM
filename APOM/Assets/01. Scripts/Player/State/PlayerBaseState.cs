using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static PlayerController;
using static UnityEngine.UI.Image;

public class PlayerBaseState : IState
{
    protected PlayerStateMachine stateMachine;
    protected readonly PlayerDefaultData defaultData;

    private float gravity = -9.8f;
    private Vector3 velocity;
    private int groundMask = (1 << LayerMask.NameToLayer("Ground"));
    private bool isGrounded;

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
        RemotePlayerSync();
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
        CheckGrounded();
        Gravity();
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
        stateMachine.movementInput = GetInputWASD();
        //stateMachine.movementInput = stateMachine.player.inputController.playerActions.Move.ReadValue<Vector2>();
    }

    private void RemotePlayerSync()
    {
        if (!stateMachine.player.inputController.isMainPlayer)
        {
            stateMachine.player.transform.rotation = stateMachine.player.inputController.TargetSyncRotation;

            Vector3 lerpPosition = Vector3.Lerp(stateMachine.player.transform.position, stateMachine.player.inputController.TargetSyncPosition, Time.deltaTime / 0.1f);
            Vector3 delta = lerpPosition - stateMachine.player.transform.position;
            stateMachine.player.characterController.Move(delta);

            //Debug.Log($"{stateMachine.player.playerID}: {stateMachine.player.inputController.TargetSyncPosition}, {stateMachine.player.inputController.TargetSyncRotation}");
        }
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
        //Debug.Log($"reciveRotation ID {stateMachine.player.playerID} : {stateMachine.player.inputController.recivePacketRotation}");
        Vector3 forward = new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
        Vector3 right = new Vector3(Mathf.Cos(radian), 0f, -Mathf.Sin(radian));

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward * stateMachine.movementInput.y + right * stateMachine.movementInput.x;
    }

    private void Gravity()
    {
        if (!isGrounded)
        {
            // 바닥에 닿아 있지 않을 때 중력 적용
            //velocity.y += gravity * Time.deltaTime;
        }
        // 속도를 위치에 적용
        stateMachine.player.transform.position += velocity * Time.deltaTime;
    }

    private void CheckGrounded()
    {
        RaycastHit hitInfo;
        isGrounded = Physics.Raycast(stateMachine.player.transform.position, Vector3.down, out hitInfo, 0.05f, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0;
        }
    }

    protected Vector3 GetCameraDirection()
    {
        float radian = stateMachine.player.inputController.recivePacketRotation * Mathf.Deg2Rad;
        //Debug.Log($"reciveRotation ID {stateMachine.player.playerID} : {stateMachine.player.inputController.recivePacketRotation}");
        Vector3 forward = new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
        Vector3 right = new Vector3(Mathf.Cos(radian), 0f, -Mathf.Sin(radian));

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        return forward;
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

    protected void DirectRotate(Vector3 direction)
    {
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Transform playerTransform = stateMachine.player.transform;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            playerTransform.rotation = targetRotation;
        }
    }

    protected Vector2 GetInputWASD()
    {
        bool[] inputWASD = stateMachine.player.inputController.reciveKeyInputs;
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

    protected bool GetInputNoramlAttack()
    {
        return stateMachine.player.inputController.reciveKeyInputs[(int)EKEYINPUT.LCLICK];
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

    protected virtual void OnMovePerformed(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnMoveCanceled(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnNormalAttackPerformed(InputAction.CallbackContext context)
    {
    }

    protected virtual void OnNormalAttackCanceled(InputAction.CallbackContext context)
    {
    }
}

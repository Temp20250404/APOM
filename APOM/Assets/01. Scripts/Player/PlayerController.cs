using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum EKEYINPUT
    {
        W = 0,
        A = 1,
        S = 2,
        D = 3,
        END
    }

    public PlayerInputs playerInputs { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
    public CinemachineFreeLook cinemachineFreeLook { get; private set; }

    private bool isMoving = false;

    private bool[] previousWASD = new bool[(int)EKEYINPUT.END];
    private bool[] currentWASD = new bool[(int)EKEYINPUT.END];

    private bool[] sendPacketWASD = new bool[(int)EKEYINPUT.END];
    public bool[] recivePacketWASD = new bool[(int)EKEYINPUT.END];

    private float previousRotation;
    private float currentRotation;

    private float sendPacketRotation;
    public float recivePacketRotation;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;

        if (GameObject.Find("Player Camera") != null)
        {
            cinemachineFreeLook = GameObject.Find("Player Camera").GetComponent<CinemachineFreeLook>();
        }
    }

    public void Update()
    {
        InputWASD();
        CheckIsMoving();
        if (isMoving)
        {
            SendRotationPacket();
        }
        RecivePacket(sendPacketWASD, sendPacketRotation);
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    void CheckIsMoving()
    {
        if (currentWASD.Contains(true))
        {
            isMoving = true;
            currentRotation = cinemachineFreeLook.m_XAxis.Value;
        }
        else
        {
            isMoving = false;
        }
    }

    void InputWASD()
    {
        Vector2 move = playerInputs.Player.Move.ReadValue<Vector2>();

        // WASD 상태 업데이트
        currentWASD[(int)EKEYINPUT.W] = move.y > 0.5f;   // W
        currentWASD[(int)EKEYINPUT.S] = move.y < -0.5f;  // S
        currentWASD[(int)EKEYINPUT.A] = move.x < -0.5f;  // A
        currentWASD[(int)EKEYINPUT.D] = move.x > 0.5f;   // D

        bool stateChanged = false;
        for (int i = 0; i < (int)EKEYINPUT.END; i++)
        {
            if (previousWASD[i] != currentWASD[i])
            {
                stateChanged = true;
                break;
            }
        }

        if (stateChanged)
        {
            SendInputPacket();

            for (int i = 0; i < (int)EKEYINPUT.END; i++)
            {
                previousWASD[i] = currentWASD[i];
            }
        }
    }

    private void SendInputPacket()
    {
        sendPacketWASD = currentWASD;
        Debug.Log("Send Input Packet: " + string.Join(", ", sendPacketWASD));
    }

    private void SendRotationPacket()
    {
        if (previousRotation != currentRotation)
        {
            sendPacketRotation = currentRotation;
            previousRotation = currentRotation;
            Debug.Log("Send Rotation Packet: " + sendPacketRotation);
        }
    }

    public void RecivePacket(bool[] _PacketWASD, float _PacketRotation)
    {
        if (_PacketWASD == null || _PacketWASD.Length != (int)EKEYINPUT.END)
        {
            Debug.LogError("Invalid packet data received.");
            return;
        }
        recivePacketWASD = _PacketWASD;
        recivePacketRotation = _PacketRotation;
    }
}

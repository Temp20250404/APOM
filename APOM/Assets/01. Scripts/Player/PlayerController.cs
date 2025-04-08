using Cinemachine;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EKEYINPUT
{
    W = 0,
    A = 1,
    S = 2,
    D = 3,
    SPACE,
    END
}

public class PlayerController : MonoBehaviour
{

    public bool isMainPlayer = false;
    public PlayerInputs playerInputs { get; private set; }
    public PlayerInputs.PlayerActions playerActions { get; private set; }
    public CinemachineFreeLook cinemachineFreeLook { get; private set; }
    GameObject pivot;

    private bool isMoving = false;

    private bool[] previousKeyInputs = new bool[(int)EKEYINPUT.END];
    private bool[] currentKeyInputs = new bool[(int)EKEYINPUT.END];

    private bool[] sendKeyInputs = new bool[(int)EKEYINPUT.END];
    [HideInInspector] public bool[] reciveKeyInputs = new bool[(int)EKEYINPUT.END];

    private float previousRotation;
    private float currentRotation;

    private float sendPacketRotation;
    [HideInInspector] public float recivePacketRotation;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
        playerActions = playerInputs.Player;

        if (GameObject.Find("Player Camera") != null)
        {
            cinemachineFreeLook = GameObject.Find("Player Camera").GetComponent<CinemachineFreeLook>();
        }

        Transform _pivot = transform.Find("Pivot");
        if (_pivot != null)
        {
            pivot = _pivot.gameObject;
        }
    }

    private void Start()
    {
        if (cinemachineFreeLook.Follow == null || cinemachineFreeLook.Follow.parent != this.transform)
        {
            cinemachineFreeLook = null;
        }
    }

    public void Update()
    {
        KeyInput();
        CheckIsMoving();
        if (isMoving)
        {
            CheckMoveRotationChange();
        }
    }

    private void OnEnable()
    {
        playerInputs.Enable();
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }

    public void SetMainPlayer()
    {
        isMainPlayer = true;

        cinemachineFreeLook.Follow = pivot.transform;
        cinemachineFreeLook.LookAt = pivot.transform;
    }

    void CheckIsMoving()
    {
        if (isMainPlayer == false)
        {
            return;
        }

        if (currentKeyInputs.Contains(true))
        {
            isMoving = true;

            currentRotation = cinemachineFreeLook.m_XAxis.Value;
        }
        else
        {
            isMoving = false;
        }
    }

    void KeyInput()
    {
        Vector2 move = playerInputs.Player.Move.ReadValue<Vector2>();

        // WASD 상태 업데이트
        currentKeyInputs[(int)EKEYINPUT.W] = move.y > 0.5f;   // W
        currentKeyInputs[(int)EKEYINPUT.S] = move.y < -0.5f;  // S
        currentKeyInputs[(int)EKEYINPUT.A] = move.x < -0.5f;  // A
        currentKeyInputs[(int)EKEYINPUT.D] = move.x > 0.5f;   // D

        bool stateChanged = false;
        for (int i = 0; i < (int)EKEYINPUT.END; i++)
        {
            if (previousKeyInputs[i] != currentKeyInputs[i])
            {
                stateChanged = true;
                break;
            }
        }

        if (stateChanged)
        {
            sendKeyInputs = currentKeyInputs;
            SendPacket();

            for (int i = 0; i < (int)EKEYINPUT.END; i++)
            {
                previousKeyInputs[i] = currentKeyInputs[i];
            }
        }
    }

    private void SendPacket()
    {
        CS_KEYINFO packet = new CS_KEYINFO();

        packet.KeyInfo = 0;
        for (int i = 0; i < (int)EKEYINPUT.END; i++)
        {
            if (sendKeyInputs[i])
            {
                packet.KeyInfo |= (1u << i);
            }
        }

        packet.CameraYaw = sendPacketRotation;

        Managers.Network.Send(packet);

        Debug.Log("Send Input Packet: " + string.Join(", ", sendKeyInputs));
    }

    private void CheckMoveRotationChange()
    {
        if (isMainPlayer == false)
        {
            return;
        }

        if (previousRotation != currentRotation)
        {
            sendPacketRotation = currentRotation;
            previousRotation = currentRotation;

            SendPacket();
            Debug.Log("Send Rotation Packet: " + sendPacketRotation);
        }
    }

    public void RecivePacket(uint _PacketKeyInputs, float _PacketRotation)
    {
        for (int i = 0; i < (int)EKEYINPUT.END; i++)
        {
            reciveKeyInputs[i] = (_PacketKeyInputs & (1u << i)) != 0;
        }

        recivePacketRotation = _PacketRotation;
    }
}

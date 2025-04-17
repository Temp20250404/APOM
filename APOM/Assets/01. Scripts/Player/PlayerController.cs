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

    public bool isMoving = false;

    private uint previousKeyInputs = 0;
    private bool[] currentKeyInputs = new bool[(int)EKEYINPUT.END];
    private uint ucurrentKeyInputs = 0;

    private uint sendKeyInputs = 0;
    [HideInInspector] public bool[] reciveKeyInputs = new bool[(int)EKEYINPUT.END];

    private float previousRotation;
    private float currentRotation;

    private float sendPacketRotation;
    [HideInInspector] public float recivePacketRotation;

    bool rotationChanged;

    public Vector3 TargetSyncPosition;
    public Quaternion TargetSyncRotation;

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
        CheckMoveRotationChange();

        

        if (isMainPlayer)
        {
            rotationChanged = (ucurrentKeyInputs != 0) && (currentRotation != previousRotation);

            if (ucurrentKeyInputs != previousKeyInputs || rotationChanged)
            {
                previousKeyInputs = ucurrentKeyInputs;
                previousRotation = currentRotation;
                Util.SendPacket<CS_KEYINFO>(packet =>
                {
                    packet.KeyInfo = sendKeyInputs;
                    packet.CameraYaw = sendPacketRotation;
                });
            }

            Util.SendPacket<CS_POSITION_SYNC>(packet =>
            {
                packet.PosX = transform.position.x;
                packet.PosY = transform.position.z;
                packet.CameraYaw = transform.rotation.eulerAngles.y;
            });
            //Debug.Log($"Send Position Packet: {transform.position.x}, {transform.position.z}, {transform.rotation.eulerAngles.y}");
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

    void KeyInput()
    {
        currentKeyInputs[(int)EKEYINPUT.W] = Input.GetKey(KeyCode.W);
        currentKeyInputs[(int)EKEYINPUT.S] = Input.GetKey(KeyCode.S);
        currentKeyInputs[(int)EKEYINPUT.A] = Input.GetKey(KeyCode.A);
        currentKeyInputs[(int)EKEYINPUT.D] = Input.GetKey(KeyCode.D);

        ucurrentKeyInputs = ChnageToUint(currentKeyInputs);

        if (ucurrentKeyInputs != 0)
        {
            isMoving = true;
        }

        if (previousKeyInputs != ucurrentKeyInputs)
        {
            sendKeyInputs = ucurrentKeyInputs;
        }
    }

    private uint ChnageToUint(bool[] _keyInputs)
    {
        uint uintKetyInput = 0;
        for (int i = 0; i < (int)EKEYINPUT.END; i++)
        {
            uintKetyInput |= _keyInputs[i] ? (1u << i) : 0;
        }

        return uintKetyInput;
    }

    private void CheckMoveRotationChange()
    {
        if (isMainPlayer == false)
        {
            return;
        }
        currentRotation = cinemachineFreeLook.m_XAxis.Value;

        if (previousRotation != currentRotation)
        {
            sendPacketRotation = currentRotation;
        }
    }

    public void ReciveKeyInfoPacket(uint _PacketKeyInputs, float _PacketRotation)
    {
        for (int i = 0; i < (int)EKEYINPUT.END; i++)
        {
            reciveKeyInputs[i] = (_PacketKeyInputs & (1u << i)) != 0;
        }

        recivePacketRotation = _PacketRotation;
    }

    public void ReciveTransformSyncPosition(SC_POSITION_SYNC packet)
    {
        TargetSyncPosition = new Vector3(packet.PosX, 0f, packet.PosY);
        //Debug.Log($"Recive Position Packet {packet.PlayerID} : {packet.PosX}, {packet.PosY}");
    }

    public void ReciveTransformSyncRotation(SC_POSITION_SYNC packet)
    {
        TargetSyncRotation = Quaternion.Euler(0f, packet.CameraYaw, 0f);
    }
}

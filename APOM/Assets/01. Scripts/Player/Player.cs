using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    public uint playerID { get; set; }

    [field: SerializeField] public PlayerSObj data { get; private set; }

    [field:Header("Animations")]
    [field:SerializeField] public PlayerAnimationData animationData { get; private set; }

    public Animator animator { get; private set; }
    public PlayerController inputController { get; private set; }
    public CharacterController characterController { get; private set; }
    private PlayerStateMachine stateMachine;

    public Camera mainCamera { get; set; }
    public Transform mainCameraTransform { get; set; }

    public PlayerStat Stat = new PlayerStat();

    public GameObject targetObject = null;
    public Vector3 targetPosition = Vector3.zero;

    private void Awake()
    {
        animationData.Initialize();
        animator = GetComponentInChildren<Animator>();

        inputController = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        stateMachine = new PlayerStateMachine(this);

        stateMachine.ChangeState(stateMachine.idleState);

        mainCamera = Camera.main;
        mainCameraTransform = mainCamera.transform;
        
    }

    private void Update()
    {
        stateMachine.StateHandleInput();
        stateMachine.StateUpdate();

        // 테스트용
        if (Input.GetKeyDown(KeyCode.F1))
        {
        }
    }

    private void FixedUpdate()
    {
        stateMachine.StatePhysicsUpdate();
    }

    public void OnAttackFinished()
    {
        stateMachine.ChangeState(stateMachine.idleState);
    }
}

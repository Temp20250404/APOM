using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //[field: SerializeField] public EnemySO SOData { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public EnemyAnimationData EnemyAnimationData { get; private set; }
    public Animator Anim { get; private set; }

    public CharacterController Controller { get; private set; }

    private EnemyStateMachine stateMachine;
   // public ForceReceiver ForceReceiver { get; private set; }

    private void Awake()
    {
        EnemyAnimationData.Initialize();

        Anim = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();

        stateMachine = new EnemyStateMachine(this);
    }
    void Start()
    {
        stateMachine.ChangeState(stateMachine.EnemyIdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }
}
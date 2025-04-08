using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [field: SerializeField] public BossData SOData { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public BossAnimationData BossAnimationData { get; private set; }
    public Animator Anim { get; private set; }
    public CharacterController Controller { get; private set; }

    private BossStateMachine stateMachine;
    public BossAI bossAI;

    [Header("Condition")]
    [SerializeField] private float currentHealth;

    private void Awake()
    {
        BossAnimationData.Initialize();

        Anim = GetComponentInChildren<Animator>();
        Controller = GetComponent<CharacterController>();

        bossAI = GetComponent<BossAI>();
        stateMachine = new BossStateMachine(this);
    }
    void Start()
    {
        stateMachine.ChangeState(BossState.Idle);
        currentHealth = SOData.BossConditions.Health;
    }

    private void Update()
    {
        stateMachine.Update();
    }
}

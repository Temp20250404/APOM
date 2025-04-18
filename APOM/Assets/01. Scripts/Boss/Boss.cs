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

    public BossStateMachine StateMachine { get { return stateMachine; } }
    public BossAI bossAI;

    public uint bossID { get; set; } = 0;

    [Header("Condition")]
    [SerializeField] private float currentHealth;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        bossAI = GetComponent<BossAI>();
        Controller = GetComponent<CharacterController>();
        BossAnimationData.Initialize();
        //bossAI.InitSkillsAnimationHash(BossAnimationData);
        stateMachine = new BossStateMachine(this);
        
    }
    void Start()
    {
        stateMachine.ChangeState(BossState.Idle);
        currentHealth = SOData.BossConditions.Health;
    }

    private void Update()
    {
        stateMachine.StateUpdate();
    }
}

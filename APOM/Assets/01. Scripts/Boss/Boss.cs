using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Boss : MonoBehaviour
{
    [field: SerializeField] public BossData SOData { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public BossAnimationData BossAnimationData { get; private set; }
    public Animator Anim { get; private set; }

    private BossStateMachine stateMachine;

    public BossStateMachine StateMachine { get { return stateMachine; } }
    public BossAI bossAI;

    public uint bossID { get; set; } = 0;

    [Header("Condition")]
    [SerializeField] public float currentHealth;

    private void Awake()
    {
        Anim = GetComponentInChildren<Animator>();
        bossAI = GetComponent<BossAI>();
        BossAnimationData.Initialize();
        //bossAI.InitSkillsAnimationHash(BossAnimationData);
        stateMachine = new BossStateMachine(this);
        
    }
    void Start()
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
        packet.BossID = stateMachine.Boss.bossID;
        packet.BossState = (int)BossState.Idle;
        Managers.Network.Send(packet);

        currentHealth = SOData.BossConditions.Health;
    }

    private void Update()
    {
        stateMachine.StateUpdate();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10.0f);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        bossAI.UpdatePhase(currentHealth, SOData.BossConditions.Health);

        if (currentHealth <= 0)
        {
            CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
            packet.BossID = stateMachine.Boss.bossID;
            packet.BossState = (int)BossState.Die;
            Managers.Network.Send(packet);
        }
    }
}

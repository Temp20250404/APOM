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
        stateMachine = new BossStateMachine(this);
    }
    void Start()
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
        packet.BossID = bossID;
        packet.BossState = (int)BossState.Idle;
        packet.CurSpeed = 0f;
        packet.MaxHp = (uint)SOData.BossConditions.Health;
        packet.CurrentHp = (uint)currentHealth;
        Managers.Network.Send(packet);
    }

    private void Update()
    {
        stateMachine.StateUpdate();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TakeDamage(10.0f);
        }
    }

    //public void TakeDamage(float damage)
    //{
    //    currentHealth -= damage;
    //    bossAI.UpdatePhase(currentHealth, SOData.BossConditions.Health);

    //    if (currentHealth <= 0)
    //    {
    //        CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
    //        packet.BossID = bossID;
    //        packet.BossState = (uint)BossState.Die;
    //        packet.CurSpeed = 0f;
    //        Managers.Network.Send(packet);
    //    }
    //}

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        bossAI.UpdatePhase(currentHealth, SOData.BossConditions.Health);

        // 체력 UI 및 서버 정보 갱신
        //SendHpPacket();

        // 체력이 0 이하일 경우 사망 처리
        if (currentHealth <= 0)
        {
            SendPhasePacket(BossState.Die, 0f);
        }
    }

    //private void SendHpPacket()
    //{
    //    CS_BOSS_HP_UPDATE hpPacket = new CS_BOSS_HP_UPDATE();
    //    hpPacket.BossID = bossID;
    //    hpPacket.CurrentHp = (uint)Mathf.Max(0, currentHealth);
    //    hpPacket.MaxHp = (uint)SOData.BossConditions.Health;

    //    Managers.Network.Send(hpPacket);
    //}

    private void SendPhasePacket(BossState state, float curSpeed)
    {
        CS_BOSS_PHASE packet = new CS_BOSS_PHASE();
        packet.BossID = bossID;
        packet.BossState = (uint)state;
        packet.CurSpeed = curSpeed;
        packet.CurrentHp = (uint)Mathf.Max(0, currentHealth);
        packet.MaxHp = (uint)SOData.BossConditions.Health;
        // 위치, 목표 좌표도 필요시 설정

        Managers.Network.Send(packet);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossAnimationData
{
    [SerializeField] private string ChasingParameterName = "Chasing";
    [SerializeField] private string IdleParameterName = "Idle";
    [SerializeField] private string AttackParameterName = "Attack";
    [SerializeField] private string Attack2ParameterName = "Attack2";
    [SerializeField] private string Attack_ParameterName = "@Attack";
    [SerializeField] private string WalkParameterName = "Walk";
    [SerializeField] private string BossSkill1ParameterName = "BossSkill1";
    [SerializeField] private string BossSkill2ParameterName = "BossSkill2";
    [SerializeField] private string BossSkill3ParameterName = "BossSkill3";
    [SerializeField] private string BossSkill_ParameterName = "@BossSkill";

    public int ChasingParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int Attack2ParameterHash { get; private set; }
    public int Attack_ParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }
    public int BossSkill1ParameterHash { get; private set; }
    public int BossSkill2ParameterHash { get; private set; }
    public int BossSkill3ParameterHash { get; private set; }
    public int BossSkill_ParameterHash { get; private set; }

    public void Initialize()
    {
        ChasingParameterHash = Animator.StringToHash(ChasingParameterName);
        IdleParameterHash = Animator.StringToHash(IdleParameterName);
        AttackParameterHash = Animator.StringToHash(AttackParameterName);
        Attack2ParameterHash = Animator.StringToHash(Attack2ParameterName);
        Attack_ParameterHash = Animator.StringToHash(Attack_ParameterName);
        WalkParameterHash = Animator.StringToHash(WalkParameterName);
        BossSkill1ParameterHash = Animator.StringToHash(BossSkill1ParameterName);
        BossSkill2ParameterHash = Animator.StringToHash(BossSkill2ParameterName);
        BossSkill3ParameterHash = Animator.StringToHash(BossSkill3ParameterName);
        BossSkill_ParameterHash = Animator.StringToHash(BossSkill_ParameterName);
    }
}

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

    public int ChasingParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }
    public int Attack2ParameterHash { get; private set; }
    public int Attack_ParameterHash { get; private set; }
    public int WalkParameterHash { get; private set; }

    public void Initialize()
    {
        ChasingParameterHash = Animator.StringToHash(ChasingParameterName);
        IdleParameterHash = Animator.StringToHash(IdleParameterName);
        AttackParameterHash = Animator.StringToHash(AttackParameterName);
        Attack2ParameterHash = Animator.StringToHash(Attack2ParameterName);
        Attack_ParameterHash = Animator.StringToHash(Attack_ParameterName);
        WalkParameterHash = Animator.StringToHash(WalkParameterName);
    }
}

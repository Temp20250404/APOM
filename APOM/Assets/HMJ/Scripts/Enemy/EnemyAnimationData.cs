using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyAnimationData
{
    [SerializeField] private string ChasingParameterName = "Chasing";
    [SerializeField] private string IdleParameterName = "Idle";
    [SerializeField] private string AttackParameterName = "Attack";

    public int ChasingParameterHash { get; private set; }
    public int IdleParameterHash { get; private set; }
    public int AttackParameterHash { get; private set; }

    public void Initialize()
    {
        ChasingParameterHash = Animator.StringToHash(ChasingParameterName);
        IdleParameterHash = Animator.StringToHash(IdleParameterName);
        AttackParameterHash = Animator.StringToHash(AttackParameterName);
    }
}

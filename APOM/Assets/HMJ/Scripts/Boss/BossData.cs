using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BossGroundData
{
    [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;

    [field: Header("WalkData")]
    [field: SerializeField][field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f;

    [field: Header("ChasingData")]
    [field: SerializeField][field: Range(0f, 2f)] public float ChasingSpeedModifier { get; private set; } = 1f;
}

[Serializable]
public class BossConditions
{
    [field: SerializeField] public float Health { get; set; } = 100f;
}

[CreateAssetMenu(fileName = "Boss", menuName = "Boss/Boss")]
public class BossData : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 1.5f;
    [field: SerializeField] public BossGroundData GroundData { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
    [field: SerializeField] public  BossConditions BossConditions{ get; private set; }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyGroundData
{
    [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;

    [field: Header("WalkData")]
    [field: SerializeField][field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f;

    [field: Header("ChasingData")]
    [field: SerializeField][field: Range(0f, 2f)] public float ChasingSpeedModifier { get; private set; } = 1f;
}


[CreateAssetMenu(fileName = "Enemy", menuName = "Enemys/Enemy")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 1.5f;
    [field: SerializeField] public EnemyGroundData GroundData { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }
}

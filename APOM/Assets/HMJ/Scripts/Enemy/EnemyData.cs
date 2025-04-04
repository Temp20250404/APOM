using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyGroundData
{
    [field: SerializeField][field: Range(0f, 25f)] public float BaseSpeed { get; private set; } = 5f;
    // 회전 변화 크기(민감도)
    //[field: SerializeField][field: Range(0f, 25f)] public float BaseRotationDamping { get; private set; } = 1f;

    [field: Header("IdleData")]

    // BaseSpeed * WalkSpeedModifier 가 WalkSpeed

    [field: Header("WalkData")]
    [field: SerializeField][field: Range(0f, 2f)] public float WalkSpeedModifier { get; private set; } = 0.225f;


    // BaseSpeed * RunSpeedModifier 가 RunSpeed
    [field: Header("RunData")]
    [field: SerializeField][field: Range(0f, 2f)] public float ChasingSpeedModifier { get; private set; } = 1f;
}


[CreateAssetMenu(fileName = "Enemy", menuName = "Characters/Enemy")]
public class EnemyData : ScriptableObject
{
    [field: SerializeField] public float PlayerChasingRange { get; private set; } = 10f;
    [field: SerializeField] public float AttackRange { get; private set; } = 1.5f;
    [field: SerializeField] public EnemyGroundData GroundData { get; private set; }

    //[field: SerializeField][field: Range(0f, 3f)] public float ForceTransitionTime { get; private set; }
    //[field: SerializeField][field: Range(-10f, 10f)] public float Force { get; private set; }
    [field: SerializeField] public int Damage { get; private set; }

    //[field: SerializeField][field: Range(0f, 1f)] public float DealingStartTransitionTime { get; private set; }
    //[field: SerializeField][field: Range(0f, 1f)] public float DealingEndTransitionTime { get; private set; }
}

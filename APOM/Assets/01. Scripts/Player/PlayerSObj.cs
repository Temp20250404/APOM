using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerDefaultData
{
    [field: Header("BaseData")]
    [field: SerializeField][field: Range(0.2f, 2f)] public float camSensitivity { get; set; } = 1f;
    [field: SerializeField][field: Range(0f, 25f)] public float baseSpeed { get; set; } = 6f;   
    [field: SerializeField][field: Range(0f, 25f)] public float baseRotationDamping { get; set; } = 10f;

    [field: Header("IdleData")]

    [field: Header("MoveData")]
    [field: SerializeField][field: Range(0f, 2f)] public float moveSpeedModifier { get; set; } = 1f;
}

[CreateAssetMenu(fileName = "Player")]
public class PlayerSObj : ScriptableObject
{
    [field: SerializeField] public PlayerDefaultData defaultData { get; set; }
}

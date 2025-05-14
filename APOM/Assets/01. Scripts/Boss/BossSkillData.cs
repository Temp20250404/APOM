using UnityEngine;

[CreateAssetMenu(fileName = "BossSkillData", menuName = "Boss/SkillData")]
public class BossSkillData : ScriptableObject
{
    public BossState SkillState;
    public float Cooldown;
    public float Duration;
    public GameObject EffectPrefab;
    public Vector3 Offset;
    public Vector3 Scale;
}
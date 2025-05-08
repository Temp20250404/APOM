using UnityEngine;

[CreateAssetMenu(fileName = "SkillAreaData", menuName = "Boss/SkillAreaData", order = 0)]
public class SkillAreaData : ScriptableObject
{
    [Header("General")]
    public float radius = 2f;
    public float duration = 1.5f;
    public Vector3 offset = new Vector3(0, 2, 0);

    [Header("Skill3 Growing Effect")]
    public bool useGrowEffect = false;
    public float growTime = 6f;

    [Header("Prefabs")]
    public GameObject conePrefab;
    public GameObject areaBasePrefab;
    public GameObject areaGrowPrefab;
}
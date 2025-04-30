using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelAnimEvent : MonoBehaviour
{
    private Player player;

    [SerializeField] private Transform[] arrowSpawnPoints;
    [SerializeField] private GameObject arrowPrefab;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void OnAttackFinished()
    {
        player.OnAttackFinished();
    }

    public void OnShootArrow(int _spawnPoint)
    {
        if (arrowSpawnPoints == null || arrowPrefab == null)
        {
            Debug.Log("화살 프리팹이나 스폰 포인트가 지정되지않았습니다.");
            return;
        }

        GameObject arrow = Instantiate(arrowPrefab,
            arrowSpawnPoints[_spawnPoint].position,
            arrowSpawnPoints[_spawnPoint].rotation
        );


    }
}

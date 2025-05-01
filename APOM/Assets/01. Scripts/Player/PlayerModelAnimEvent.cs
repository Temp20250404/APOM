using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelAnimEvent : MonoBehaviour
{
    private Player player;

    [SerializeField] private Transform[] arrowSpawnPoints;
    [SerializeField] private GameObject arrowPrefab;

    private Vector3 target;
    private Vector3 dir;
    float angle;
    Quaternion targetRot;
    float angleDifferent;

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

        if (player.targetObject != null)
        {
            target = player.targetObject.transform.position;
        }
        else if (player.targetPosition != Vector3.zero)
        {
            target = player.targetPosition;
        }
        else
        {
            target = player.transform.position + player.transform.forward * 10f;
        }

        dir = (target - arrowSpawnPoints[_spawnPoint].position).normalized;
        angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        targetRot = Quaternion.Euler(0f, 0f, angle);

        GameObject go = Instantiate(arrowPrefab,
            arrowSpawnPoints[_spawnPoint].position,
            arrowSpawnPoints[_spawnPoint].rotation
        );
    }
}

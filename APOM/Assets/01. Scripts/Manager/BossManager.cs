using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : IManager
{
    private GameObject BossPrefab;

    private Dictionary<uint, Boss> bossList = new Dictionary<uint, Boss>();

    public void Init()
    {
        bossList.Clear();
        BossPrefab = Resources.Load<GameObject>("Boss/Boss"); // 기본 보스 프리팹
    }

    public void Clear()
    {
        bossList.Clear();
    }

    public void SpawnBoss(SC_CREATE_MONSTER _packet)
    {
        if (bossList.ContainsKey(_packet.AiID))
        {
            return;
        }

        Vector3 spawnPosition = new Vector3(_packet.MonsterPos.PosX, _packet.MonsterPos.PosY, _packet.MonsterPos.PosZ);
        GameObject go = Object.Instantiate(BossPrefab, spawnPosition, Quaternion.identity);
        Boss boss = Util.GetOrAddComponent<Boss>(go);
        boss.bossID = _packet.AiID;
        AddBoss(_packet.AiID, boss);
    }

    public Boss GetBoss(uint id)
    {
        bossList.TryGetValue(id, out Boss boss);
        return boss;
    }

    public void AddBoss(uint _id, Boss boss)
    {
        if (!bossList.ContainsKey(_id))
        {
            bossList.Add(_id, boss);
        }
    }

    public void RemoveBoss(uint _id)
    {
        if(bossList.TryGetValue(_id, out Boss boss))
        {
            bossList.Remove(_id);
            GameObject.Destroy(boss);

            Debug.Log($"몬스터 {_id} 삭제 성공");
        }
        else
            Debug.Log($"몬스터 {_id} 삭제 실패");
    }
}

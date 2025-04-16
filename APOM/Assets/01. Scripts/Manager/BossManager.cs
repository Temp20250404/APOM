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
        BossPrefab = Resources.Load<GameObject>("Boss/Boss"); // �⺻ ���� ������
    }

    public void Clear()
    {
        bossList.Clear();
    }

    //public void SpawnBoss(SC_SPAWN_BOSS _packet)
    //{
    //    if (bossList.ContainsKey(_packet.BOSSID))
    //    {
    //        return;
    //    }

    //    Vector3 spawnPosition = new Vector3(_packet.PosX, 0f, _packet.PosY);
    //    GameObject go = Object.Instantiate(BossPrefab, spawnPosition, Quaternion.identity);
    //    Boss boss = Util.GetOrAddComponent<Boss>(go);
    //    boss.myBossID = _packet.BossID;
    //    AddBoss(_packet.PlayerID, boss);
    //}

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

            Debug.Log($"�÷��̾� {_id} ���� ����");
        }
        else
            Debug.Log($"�÷��̾� {_id} ���� ����");
    }
}

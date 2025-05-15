using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Boss = 1,
    Minion = 2,
}
public class BossManager : IManager
{
    private Dictionary<uint, Boss> bossList = new Dictionary<uint, Boss>();

    private Dictionary<MonsterType, string> monsterPrefabPathMap = new()
    {
        { MonsterType.Boss, "Boss/Dragon" },
        { MonsterType.Minion, "Boss/Boss" }
    };

    public void Init()
    {
        bossList.Clear();

        SendCreatePacket(MonsterType.Boss, new Vector3(53.67f, 1.21f, -33.59f));
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

        // 패킷에서 몬스터 타입 확인
        MonsterType type = (MonsterType)_packet.MonsterType;

        // 프리팹 경로 확인
        if (!monsterPrefabPathMap.TryGetValue(type, out string prefabPath))
        {
            Debug.LogError($"몬스터 타입에 대한 프리팹 경로가 없습니다: {type}");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"프리팹 로드 실패: {prefabPath}");
            return;
        }
        // 몬스터 위치 설정
        Vector3 spawnPosition = new Vector3
            (_packet.MonsterPos.PosX, _packet.MonsterPos.PosY, _packet.MonsterPos.PosZ);

        GameObject go = Object.Instantiate(prefab, spawnPosition, Quaternion.identity);
        Boss boss = Util.GetOrAddComponent<Boss>(go);
        boss.bossID = _packet.AiID;

        var ui = Managers.UI.GetPopupUI<UI_BossCondition>();
        ui.SetBossNameText(boss.SOData.BossName);
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

    private void SendCreatePacket(MonsterType type, Vector3 transform)
    {
        CS_CREATE_MONSTER packet = new CS_CREATE_MONSTER
        {
            MonsterType = (uint)type,
            MonsterPos = new Position
            {
                PosX = transform.x,
                PosY = transform.y,
                PosZ = transform.z
            },
        };
        Managers.Network.Send(packet);
    }
}

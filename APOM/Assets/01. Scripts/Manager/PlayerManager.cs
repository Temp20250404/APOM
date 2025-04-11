using Game;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IManager
{
    private Dictionary<uint, Player> loginPlayerList = new Dictionary<uint, Player>();

    public GameObject playerprefab;

    bool isFirstSpawn = true;

    public void Init()
    {
        loginPlayerList.Clear();

        playerprefab = Resources.Load<GameObject>("Player/Player");
    }

    public void Clear()
    {
        foreach (var player in loginPlayerList.Values)
        {
            GameObject.Destroy(player);
        }
        loginPlayerList.Clear();
    }

    public void SpawnPlayer(SC_SPAWN_CHARACTER _packet)
    {
        if (loginPlayerList.ContainsKey(_packet.PlayerID))
        {
            return;
        }
        Vector3 spawnPosition = new Vector3(_packet.PosX, 0f, _packet.PosY);
        GameObject go = Object.Instantiate(playerprefab, spawnPosition, Quaternion.identity);
        Player player = Util.GetOrAddComponent<Player>(go);
        player.playerID = _packet.PlayerID;
        AddPlayer(_packet.PlayerID, player);

        if (isFirstSpawn)
        {
            player.inputController.SetMainPlayer();
            isFirstSpawn = false;
        }
    }

    public void AddPlayer(uint _id, Player _player)
    {
        if (!loginPlayerList.ContainsKey(_id))
        {
            loginPlayerList.Add(_id, _player);
        }
    }

    public void RemovePlayer(uint _id)
    {
        if (loginPlayerList.TryGetValue(_id, out Player _player))
        {
            loginPlayerList.Remove(_id);
            GameObject.Destroy(_player);

            Debug.Log($"플레이어 {_id} 삭제 성공");
        }
        else
            Debug.Log($"플레이어 {_id} 삭제 실패");
    }

    public Player GetPlayer(uint _playerID)
    {
        loginPlayerList.TryGetValue(_playerID, out Player _player);
        return _player;
    }

    public IReadOnlyCollection<Player> GetAllPlayers()
    {
        return loginPlayerList.Values;
    }
}

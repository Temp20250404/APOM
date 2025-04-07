using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : IManager
{
    private Dictionary<uint, GameObject> loginPlayerList = new Dictionary<uint, GameObject>();

    public void Init()
    {
        loginPlayerList.Clear();
    }

    public void Clear()
    {
        foreach (var player in loginPlayerList.Values)
        {
            GameObject.Destroy(player);
        }
        loginPlayerList.Clear();
    }

    public void AddPlayer(uint _id, GameObject _player)
    {
        if (!loginPlayerList.ContainsKey(_id))
        {
            loginPlayerList.Add(_id, _player);
        }
    }

    public void RemovePlayer(uint _id)
    {
        if (loginPlayerList.TryGetValue(_id, out GameObject _playerObject))
        {
            loginPlayerList.Remove(_id);
            GameObject.Destroy(_playerObject);
        }
    }

    public GameObject GetPlayer(uint _playerID)
    {
        loginPlayerList.TryGetValue(_playerID, out GameObject _playerObject);
        return _playerObject;
    }

    public IReadOnlyCollection<GameObject> GetAllPlayers()
    {
        return loginPlayerList.Values;
    }
}

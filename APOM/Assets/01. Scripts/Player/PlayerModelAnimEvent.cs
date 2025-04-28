using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelAnimEvent : MonoBehaviour
{
    private Player player;

    void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    public void OnAttackFinished()
    {
        player.OnAttackFinished();
    }
}

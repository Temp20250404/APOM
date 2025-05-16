using Game;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectDamage : MonoBehaviour
{
    public float damageAmount = 30f;
    private HashSet<uint> damagedIDs = new();

    private void OnEnable()
    {
        // 이펙트가 다시 켜질 때마다 충돌 기록 초기화
        damagedIDs.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
       TakeDamage(other);
    }

    private void TakeDamage(Collider other)
    {
        Player p = other.GetComponent<Player>();

        if (p != null && !damagedIDs.Contains(p.playerID))
        {
            damagedIDs.Add(p.playerID);

            CS_MONSTER_ATTACK packet = new CS_MONSTER_ATTACK
            {
                PlayerID = p.playerID,
                Damage = (uint)damageAmount
            };
            Managers.Network.Send(packet);
        }
    }
}

using Game;
using System.Collections.Generic;
using UnityEngine;

public class BossBasicAttack : MonoBehaviour
{
    public float damage = 10f;
    private HashSet<uint> damagedIDs = new();

    // 애니메이션 이벤트에서 호출
    public void ResetHitList()
    {
        damagedIDs.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        Player p = other.GetComponent<Player>();
        if (p != null && !damagedIDs.Contains(p.playerID))
        {
            damagedIDs.Add(p.playerID);

            CS_MONSTER_ATTACK packet = new CS_MONSTER_ATTACK
            {
                PlayerID = p.playerID,
                Damage = (uint)damage
            };
            Managers.Network.Send(packet);
        }
    }
}

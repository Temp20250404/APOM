using Game;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffectDamage : MonoBehaviour
{
    public float damageAmount = 30f;

    private void OnTriggerEnter(Collider other)
    {
       TakeDamage(other);
    }

    private void TakeDamage(Collider other)
    {
        if (other.TryGetComponent<Player>(out var damageable))
        {
            CS_MONSTER_ATTACK packet = new CS_MONSTER_ATTACK
            {
                PlayerID = damageable.playerID,
                Damage = (uint)damageAmount
            };
        }
    }
}

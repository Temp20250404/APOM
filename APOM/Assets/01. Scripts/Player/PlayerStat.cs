using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerStat
{
    public float atk;

    public float str;

    public float weaponAtk;

    public float critRate;
    public float critDamage;

    public float baseHp;
    public float maxHp;
    public float currentHp;

    public float def;

    public float atkDelay;

    public int baseMp;
    public int maxMp;
    public int currentMp;

    public float moveSpeed;

    private float minDamageRate = 0.98f;
    private float maxDamageRate = 1.02f;

    public float CulDamage(float _skillDamage, bool _isCrit)
    {
        float mulCritDamage;
        if (_isCrit)
        {
            mulCritDamage = critDamage;
        }
        else
        {
            mulCritDamage = 1f;
        }

        atk = (str * 1.5f) + weaponAtk * mulCritDamage;

        float finalDamage = _skillDamage * atk * Random.Range(minDamageRate, maxDamageRate);

        return finalDamage;
    }
}

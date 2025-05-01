using APOM_Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;

public class PlayerStat
{
    public int level { get; private set; } = 1;

    public EJOB job { get; private set; }
    public float atk { get; private set; }

    public float str { get; private set; }

    public float weaponAtk { get; private set; }

    public float critRate { get; private set; }
    public float critDamage { get; private set; }

    public float baseHp { get; private set; }
    public float maxHp;
    public float currentHp;

    public float def { get; private set; }

    public float BaseAtkDelay { get; private set; }
    public float atkDelay = 1;

    public int baseMp { get; private set; }
    public int maxMp;
    public int currentMp;

    public float rayRange { get; private set; }

    public float moveSpeed { get; private set; }

    private float minDamageRate = 0.98f;
    private float maxDamageRate = 1.02f;

    public void SetStats(uint _jobIndex)
    {
        Managers.Data.jobBaseStatsData.GetDictionary().TryGetValue((int)_jobIndex, out JobBaseStats_Data _Data);

        if (_Data == null)
        {
            Debug.LogError($"JobBaseStatsData not found for index: {_jobIndex}");
            return;
        }

        job = _Data.job;
        atk = _Data.atk;
        str = _Data.str;
        weaponAtk = _Data.weaponAtk;
        critRate = _Data.critPercent;
        critDamage = _Data.critDamage;
        baseHp = _Data.health;
        maxHp = baseHp;
        currentHp = baseHp;
        def = _Data.def;
        BaseAtkDelay = _Data.atkDelay;
        moveSpeed = _Data.movesp;
        baseMp = _Data.mana;
        maxMp = baseMp;
        currentMp = baseMp;
        rayRange = _Data.rayRange;
    }

    public uint CulSkillDamage(float _skillDamage)
    {
        float mulCritDamage;
        bool isCrit = UnityEngine.Random.Range(0f, 1f) < critRate;

        if (isCrit)
        {
            mulCritDamage = critDamage;
        }
        else
        {
            mulCritDamage = 1f;
        }

        atk = (str * 1.5f) + weaponAtk * mulCritDamage;

        float finalDamage = _skillDamage * atk * UnityEngine.Random.Range(minDamageRate, maxDamageRate);
        if (finalDamage < 1)
        {
            finalDamage = 1;
        }
        else if (finalDamage <= UInt32.MaxValue)
        {
            finalDamage = UInt32.MaxValue;
        }

        return (uint)finalDamage;
    }

    public void TakeDamage(float _damage)
    {
        currentHp -= _damage;
        if (currentHp < 0)
        {
            currentHp = 0;
        }
    }

    public void LevelUp()
    {
        level++;
    }

    public void SetLevel(int _level)
    {
        level = _level;
    }
}

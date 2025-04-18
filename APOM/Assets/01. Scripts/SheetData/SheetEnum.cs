using GoogleSheet.Core.Type;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[UGS(typeof(EBOOL))]
public enum EBOOL
{
    FALSE,
    TRUE
}

[UGS(typeof(EITEMTYPE))]
public enum EITEMTYPE
{
    NONE,
    EQUIPMENT,
    CONSUMABLE,
    ENHANCEMENTMATERIAL
}

[UGS(typeof(ESUBTYPE))]
public enum ESUBTYPE
{
    NONE,
    WEAPON,
    HELMET,
    ARMOR,
    GLOVES,
    SHOES,
    PENDANT,
    EARRING,
    RING,
    HP,
    MP,
    BUFF,
    GEAR,
    ACCESSORY
}

[UGS(typeof(EGRADE))]
public enum EGRADE
{
    NONE,
    COMMON,
    UNCOMMON,
    RARE,
    LEGENDARY
}

[UGS(typeof(EJOB))]
public enum EJOB
{
    NONE,
    WHOLE,
    ARCHER,
    LANCER
}

[UGS(typeof(ECTYPE))]
public enum ECTYPE
{
    NONE,
    HP,
    MP,
    BUFF
}
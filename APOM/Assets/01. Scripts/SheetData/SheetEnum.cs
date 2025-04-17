using GoogleSheet.Core.Type;
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
    CONSUMABLE
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

[UGS(typeof(EEQUIPTYPE))]
public enum EEQUIPTYPE
{
    NONE,
    WEAPON,
    HELMET,
    ARMOR,
    GLOVES,
    SHOES,
    PENDANT,
    EARRING,
    RING
}

[UGS(typeof(EJOB))]
public enum EJOB
{
    NONE,
    WHOLE,
    ARCHER,
    LANCER
}

[UGS(typeof(ECONSUMTYPE))]
public enum ECONSUMTYPE
{
    NONE,
    HP,
    MP,
    BUFF
}
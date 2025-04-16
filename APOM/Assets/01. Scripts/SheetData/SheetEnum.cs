using GoogleSheet.Core.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
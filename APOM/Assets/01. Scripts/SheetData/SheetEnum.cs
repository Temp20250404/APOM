using GoogleSheet.Core.Type;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SheetEnum
{
    [UGS(typeof(EGRADE))]
    public enum EGRADE
    {
        COMMON,
        UNCOMMON,
        RARE,
        LEGENDARY
    }

    [UGS(typeof(EEQUIPTYPE))]
    public enum EEQUIPTYPE
    {
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
        ARCHER,
        LANCER
    }

    [UGS(typeof(ECTYPE))]
    public enum ECTYPE
    {
        HP,
        MP,
        BUFF
    }
}
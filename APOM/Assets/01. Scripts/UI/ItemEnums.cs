using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemEnums : MonoBehaviour // 아이템 분류용
{
    public enum ItemCategory
    {
        Weapon,
        Expendable,
        Ingredient,
        Etc
    }

    public enum ItemSubType
    {
        Weapon,
        Helmet,
        Armor,
        Shoes,
        Gloves,
        Necklace,
        Ring,
        Earring,
        
        HP,
        MP,
        
        None
    }
}

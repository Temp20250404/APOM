using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemEnums;

public enum ItemCategory
{
    Equipment,
    Consumable,
    Material,
    Etc
}

public enum EquipmentType
{
    Weapon,
    Consum, // 한벌옷
    Shoes,
    Gloves,
    Necklace,
    Ring,
    Earring
}

[System.Serializable]
public class Item : UI_Popup // 아이템 정보
{
    public string name; // 아이템 이름
    public Sprite icon; // 아이템 아이콘
    public string description; // 설명
    public int count; // 개수

    public ItemCategory category; // 장비 소모품 강화재료 기타

    public EquipmentType? subType; // 장비일 경우만 사용

    public int price; // 가격
    public int id; // 아이템 고유 아이디
}

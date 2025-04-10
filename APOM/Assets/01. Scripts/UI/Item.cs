using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemEnums;

public class Item : MonoBehaviour // 아이템 정보
{
    public string name; // 아이템 이름
    public ItemCategory category; // 아이템 카테고리
    public ItemSubType subType; //아이템 세분화
    public Sprite icon; // 아이템 이미지
}

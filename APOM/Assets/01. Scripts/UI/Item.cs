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
    Consum, // �ѹ���
    Shoes,
    Gloves,
    Necklace,
    Ring,
    Earring
}

[System.Serializable]
public class Item : UI_Popup // ������ ����
{
    public string name; // ������ �̸�
    public Sprite icon; // ������ ������
    public string description; // ����
    public int count; // ����

    public ItemCategory category; // ��� �Ҹ�ǰ ��ȭ��� ��Ÿ

    public EquipmentType? subType; // ����� ��츸 ���

    public int price; // ����
    public int id; // ������ ���� ���̵�
}

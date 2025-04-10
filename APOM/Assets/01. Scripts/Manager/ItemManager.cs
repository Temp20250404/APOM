using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ItemEnums;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<Item> Items = new List<Item>();

    private void Awake()
    {
        Instance = this;

        // 임시용 아이템
        Items.Add(new Item { name = "Sword", category = ItemCategory.Weapon, subType = ItemSubType.Weapon });
        Items.Add(new Item { name = "Gloves", category = ItemCategory.Weapon, subType = ItemSubType.Gloves });
        Items.Add(new Item { name = "Health Potion", category = ItemCategory.Expendable, subType = ItemSubType.HP });
        Items.Add(new Item { name = "Mana Potion", category = ItemCategory.Expendable, subType = ItemSubType.MP });
    }

    public void SortInventory()
    {
        Items.Sort(CompareItems);
    }

    private int CompareItems(Item a, Item b)
    {
        int orderA = GetSortPriority(a);
        int orderB = GetSortPriority(b);

        if (orderA != orderB)
            return orderA.CompareTo(orderB);

        return string.Compare(a.name, b.name);
    }

    private int GetSortPriority(Item item)
    {
        int categoryOrder = item.category switch
        {
            ItemCategory.Weapon => 0,
            ItemCategory.Expendable => 1,
            ItemCategory.Ingredient => 2,
            ItemCategory.Etc => 3,
            _ => 4
        };

        int subOrder = item.subType switch
        {
            ItemSubType.Weapon => 0,
            ItemSubType.Armor => 1,
            ItemSubType.Shoes => 2,
            ItemSubType.Gloves => 3,
            ItemSubType.Necklace => 4,
            ItemSubType.Ring => 5,
            ItemSubType.Earring => 6,
            ItemSubType.HP => 0,
            ItemSubType.MP => 1,
            _ => 99
        };

        return categoryOrder * 100 + subOrder;
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_Inventory : UI_Popup
{
    enum Buttons
    {
        CloseButton, //오브젝트의 이름
        ArrayButton,
        EquipmentButton,
        ConsumableButton,
        MaterialButton,
        EtcButton
    }

    private List<Item> originalItemList = new List<Item>();
    private List<Item> currentItemList = new List<Item>();

    private HashSet<ItemCategory> activeCategories = new HashSet<ItemCategory>();

    public override void Init()
    {
        base.Init();

        popupType = PopupType.Inventory;
        Bind<Button>(typeof(Buttons));

        // 버튼 클릭 시 닫기
        GetButton((int)Buttons.CloseButton).onClick.AddListener(() => 
        { 
            Managers.UI.ClosePopupUI();
        });

        GetButton((int)Buttons.ArrayButton).onClick.AddListener(() =>
        {
            SortInventory(currentItemList);
        });

        // 카테고리 버튼 리스너
        GetButton((int)Buttons.EquipmentButton).onClick.AddListener(() => ToggleCategory(ItemCategory.Equipment));
        GetButton((int)Buttons.ConsumableButton).onClick.AddListener(() => ToggleCategory(ItemCategory.Consumable));
        GetButton((int)Buttons.MaterialButton).onClick.AddListener(() => ToggleCategory(ItemCategory.Material));
        GetButton((int)Buttons.EtcButton).onClick.AddListener(() => ToggleCategory(ItemCategory.Etc));

        originalItemList = LoadInventory();
        currentItemList = new List<Item>(originalItemList);

        UpdateInventoryUI(currentItemList);
    }

    private void ToggleCategory(ItemCategory category)
    {
        if (activeCategories.Contains(category))
            activeCategories.Remove(category);
        else
            activeCategories.Add(category);

        if (activeCategories.Count == 0)
        {
            currentItemList = new List<Item>(originalItemList);
        }
        else
        {
            currentItemList = originalItemList
                .Where(item => activeCategories.Contains(item.category))
                .OrderBy(item => GetDetailedSortPriority(item))
                .ToList();
        }

        UpdateInventoryUI(currentItemList);
    }

    private int GetDetailedSortPriority(Item item)
    {
        if (item.category == ItemCategory.Equipment)
        {
            switch (item.subType)
            {
                case EquipmentType.Weapon: return 0;
                case EquipmentType.Consum: return 1;
                case EquipmentType.Shoes: return 2;
                case EquipmentType.Gloves: return 3;
                case EquipmentType.Necklace: return 4;
                case EquipmentType.Ring: return 5;
                case EquipmentType.Earring: return 6;
            }
        }

        else if (item.category == ItemCategory.Consumable)
        {
            if (item.name.Contains("HP")) return 0;
            if (item.name.Contains("MP")) return 1;
        }

        else if (item.category == ItemCategory.Etc)
        {
            return item.name.Contains("퀘스트") ? 0 : 1;
        }

        return 100;
    }

    private void SortInventory(List<Item> inventory)
    {
        inventory.Sort((a, b) =>
        {
            int orderA = GetDetailedSortPriority(a);
            int orderB = GetDetailedSortPriority(b);

            if (orderA != orderB)
                return orderA.CompareTo(orderB);

            return string.Compare(a.name, b.name);
        });

        Debug.Log("인벤토리 정렬 끝");
        UpdateInventoryUI(inventory);
    }

    private List<Item> LoadInventory()
    {
        return new List<Item>() // 임시용 아이템
            {
            new Item()
            {
                name = "강철검",
                category = ItemCategory.Equipment,
                subType = EquipmentType.Weapon
            },
            new Item()
            {
                name = "HP포션",
                category = ItemCategory.Consumable,
                subType = null
            }
        };
    }

    private void UpdateInventoryUI(List<Item> inventory)
    {
        // UI 갱신 코드
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Managers.UI.ShowPopupUI<UI_Inventory>();
        }
    }
}

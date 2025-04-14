using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_Inventory : UI_Popup
{
    enum Buttons
    {
        CloseButton, //오브젝트의 이름
        ArrayButton

    }

    private List<Item> item = new List<Item>();

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
            SortInventory(item);
        });
    }

    private int GetSortPriority(ItemCategory category)
    {
        switch (category)
        {
            case ItemCategory.Weapon: return 0;
            case ItemCategory.Expendable: return 1;
            case ItemCategory.Ingredient: return 2;
            case ItemCategory.Etc: return 3;
            default: return 4;
        }
    }

    public void SortInventory(List<Item> inventory)
    {
        inventory.Sort(CompareItems);
        Debug.Log("인벤토리 정렬 끝");
        // UpdateInventoryUI(); // 여기에 UI 갱신 코드 있으면 실행
    }

    private int CompareItems(Item a, Item b)
    {
        int orderA = GetSortPriority(a.category);
        int orderB = GetSortPriority(b.category);

        if (orderA != orderB)
            return orderA.CompareTo(orderB);

        return string.Compare(a.name, b.name);
    }
}

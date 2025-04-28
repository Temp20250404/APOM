using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[SerializeField] private Transform slotParent;
[SerializeField] private GameObject slotPrefab;
[SerializeField] private UI_InventoryTooltip tooltip;

private void UpdateInventoryUI(List<Item> inventory)
{
    // 기존 슬롯들 제거
    foreach (Transform child in slotParent)
        Destory(child, gameObject);

    // 새로운 슬롯 생성
    foreach (var item in inventory)
    {
        GameObject go = Instantiate(slotPrefab, slotParent);

        // 슬롯 스크립트 가져오기
        UI_InventorySlot slot = go.GetComponent<UI_InventorySlot>();
        if (slot != null)
            slot.Init(item, tooltipUI);

        // 슬롯에 아이템 이름 등 텍스트 표시 (예: 슬롯 안에 Text 컴포넌트가 있다고 가정)
        Text itemNameText = go.GetComponentInChildren<Text>();
        if (itemNameText != null)
            itemNameText.text = item.name;
    }
}
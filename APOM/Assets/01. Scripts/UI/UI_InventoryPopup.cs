using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryPopup : MonoBehaviour
{
    [SerializeField] private GameObject itemSlotPrefab;
    [SerializeField] private Transform gridParent;
    [SerializeField] private Button sortButton;
    [SerializeField] private Button closeButton;

    private void Start()
    {
        sortButton.onClick.AddListener(() =>
        {
            //UI_Inventory.Instance.SortInventory();
            RefreshUI();
        });

        closeButton.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });

        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in gridParent)
        {
            Destroy(child.gameObject);
        }

       // foreach (var item in ItemManager.Instance.Items)
        {
            var slot = Instantiate(itemSlotPrefab, gridParent).GetComponent<UI_InventorySlot>();
            //slot.SetSlot(item);
        }
    }
}

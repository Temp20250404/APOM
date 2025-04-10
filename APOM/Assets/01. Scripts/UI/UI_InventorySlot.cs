using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventorySlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private Text nameText;

    public void SetSlot(Item item)
    {
        if (icon != null)
            icon.sprite = item.icon;

        if (nameText != null)
            nameText.text = item.name;
    }
}

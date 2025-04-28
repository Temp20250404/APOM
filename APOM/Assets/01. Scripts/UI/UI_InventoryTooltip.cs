using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_InventoryTooltip : MonoBehaviour
{
    public Text nameText;
    public Text descriptionText;
    public GameObject tooltipPanel;

    private void Awake()
    {
        HideTooltip();
    }

    public void ShowTooltip(Item item, Vector3 position)
    {
        tooltipPanel.SetActive(true);
        tooltipPanel.transform.position = position;

        nameText.text = item.name;
        descriptionText.text = item.description; // Item 클래스에 description 있어야 함
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}

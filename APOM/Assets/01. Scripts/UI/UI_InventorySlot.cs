//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class UI_InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
//{
//    private Item item;
//    private UI_InventoryTooltip tooltip;

//    public void Init(Item itemData, UI_InventoryTooltip tooltipRef) 
//    {
//        item = itemData;
//        tooltip = tooltipRef;
//    }

//    public void OnPointerEnterHandler(PointerEventData eventData)
//    {
//        tooltip.ShowTooltip(item, Input.mousePosition);
//    }

//    public void OnpointerExit(PointerEventData eventData)
//    {
//        tooltip.HideTooltip();
//    }
//}

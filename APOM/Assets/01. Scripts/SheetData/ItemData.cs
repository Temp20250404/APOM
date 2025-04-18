using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ItemData : APOM_Data.Item_Data
{
    public Dictionary<int, ItemData> GetEquipmentDataDictionary()
    {
        Dictionary<int, ItemData> EquipmentDictionary = new Dictionary<int, ItemData>();
        //ItemData.GetDictionary().TryGetValue(1, out ItemData foundItem);

        return EquipmentDictionary;
    }
}

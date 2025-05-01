using APOM_Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Animations;

public class ItemData : APOM_Data.Item_Data
{
    private Dictionary<int, Item_Data> itemDataDictionary = new Dictionary<int, Item_Data>();
    private List<Item_Data> itemDataList = new List<Item_Data>();

    public void Init()
    {
        itemDataDictionary = Item_Data.GetDictionary();
        itemDataList = Item_Data.GetList();
    }

    public Dictionary<int, Item_Data> GetDictionary()
    {
        return itemDataDictionary;
    }

    public List<Item_Data> GetList()
    {
        return itemDataList;
    }
}

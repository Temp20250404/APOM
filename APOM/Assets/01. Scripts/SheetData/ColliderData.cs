using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APOM_Data;

public class ColliderData : Collider_Data
{
    private Dictionary<int, Collider_Data> colliderDataDictionary = new Dictionary<int, Collider_Data>();

    public List<Collider_Data> GetByName(string name)
    {
        colliderDataDictionary = Collider_Data.GetDictionary();

        List<Collider_Data> resultList = new List<Collider_Data>();

        foreach (var pair in colliderDataDictionary)
        {
            if (pair.Value.name == name)
            {
                Debug.Log(pair.Key);
                resultList.Add(pair.Value);
            }
                

        }

        if (resultList.Count == 0)
            Debug.LogWarning($"ColliderData: name '{name}' not found.");
        return resultList;
    }
}

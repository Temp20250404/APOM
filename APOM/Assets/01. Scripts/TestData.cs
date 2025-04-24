using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class TestData : MonoBehaviour
{
    void Awake()
    {

        // UnityGoogleSheet.Load<DefaultTable.Data.Load>(); it's same!
        // or call DefaultTable.Data.Load(); it's same!
    }

    void Start()
    {
        ; // Load Data

        foreach (var value in APOM_Data.Item_Data.GetList())
        {
            Debug.Log(value.index + "," + value.buyPrice + "," + value.weaponATK);
        }

        foreach (var value in APOM_Data.Item_Data.GetDictionary())
        {
            Debug.Log(value.Key + "," + value.Value);
        }
    }
}

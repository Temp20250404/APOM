using GoogleSheet;
using GoogleSheet.Core.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UGS;
using Unity.Jobs;
using UnityEngine;

public class DataManager : IManager
{
    public ItemData itemData { get; private set; } = new ItemData();

    public void Init()
    {
        UnityGoogleSheet.LoadAllData();

        //EGRADE grades;
        //if (EquipmentData.GetDictionary().TryGetValue(3, out APOM_Data.Equipment_Data foundItem))
        //{
        //    grades = foundItem.grade;
        //}
        //SaveAllSheetDataToSDD("EnhancementRate_Data");
        Debug.Log("DataManager Initialized");
    }
    public void Clear()
    {
        // 미구현
        Debug.Log("DataManager Cleared");
    }

    //사용하지 않게 됨 Debug때 확인용으로 일단 남겨둠
    public static Dictionary<string, Dictionary<int, ITable>> SheetDataDictionarys = new Dictionary<string, Dictionary<int, ITable>>();

    public static void SaveSheetDataToDict<T>(Func<Dictionary<int, T>> getDictionary) where T : ITable, new()
    {
        Dictionary<int, T> dict = getDictionary();

        string sheetName = typeof(T).Name;
        Debug.Log("1");

        var converted = dict.ToDictionary(kvp => kvp.Key, kvp => (ITable)kvp.Value);
        SheetDataDictionarys.Add(sheetName, converted);
    }

    public static void SaveAllSheetDataToSDD(string _sheetName)
    {
        // 예: 네임스페이스가 _sheetName인 타입들을 찾음
        var sheetTypes = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(t => t.IsClass && t.Namespace == _sheetName && typeof(ITable).IsAssignableFrom(t))
            .ToList();

        Debug.Log($"Found {sheetTypes.Count} sheet data types.");

        foreach (var type in sheetTypes)
        {
            // GetDictionary() 메서드를 찾음
            MethodInfo getDictMethod = type.GetMethod("GetDictionary", BindingFlags.Public | BindingFlags.Static);

            if (getDictMethod == null)
            {
                Debug.LogWarning($"{type.Name}타입이 GetDictionary() 메서드가 없음");
                continue;
            }

            try
            {
                Type dictType = typeof(Dictionary<,>).MakeGenericType(typeof(int), type);
                Type funcType = typeof(Func<>).MakeGenericType(dictType);

                object delegateInstance = Delegate.CreateDelegate(funcType, getDictMethod);

                // SReflection
                MethodInfo saveMethod = typeof(DataManager).GetMethod("SaveSheetDataToDict", BindingFlags.Public | BindingFlags.Static);
                if (saveMethod != null)
                {
                    MethodInfo genericSaveMethod = saveMethod.MakeGenericMethod(type);
                    // delegateInstance -> Func<Dictionary<int, T>> 형식
                    genericSaveMethod.Invoke(null, new object[] { delegateInstance });
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"{type.Name}을 Load하는 것에서 문제 발생 : {ex.Message}");
            }
        }
    }
}

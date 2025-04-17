using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefs_Clear : MonoBehaviour
{
    private void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 40), "��� ���� �ʱ�ȭ"))
        {
            ResetAllUsers();
        }
    }

    private void ResetAllUsers()
    {
        int count = PlayerPrefs.GetInt("IDCount", 0);

        for (int i = 0; i < count; i++)
        {
            PlayerPrefs.DeleteKey($"User_{i}_ID");
            PlayerPrefs.DeleteKey($"User_{i}_PW");
        }

        PlayerPrefs.DeleteKey("SavedID");
        PlayerPrefs.DeleteKey("IDCount");
        PlayerPrefs.DeleteKey("SaveIDToggle");
        PlayerPrefs.Save();

        Debug.Log(" ��� ���� ������ �ʱ�ȭ�Ǿ����ϴ�!");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    public Button button;

    private void Start()
    {
        button.onClick.AddListener(() => Managers.BossManager.SendCreatePacket(MonsterType.Boss, new Vector3(53.67f, 1.21f, -33.59f)));
    }
}

using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UI_Boss_Objects
{
    HPFill,
    BuffDebuffContainer,
    BossNameText
}
public class UI_BossCondition : UI_Popup
{
    private Image hpFill;
    private GameObject buffDebuffContainer;
    private TextMeshProUGUI bossNameText;

    private GameObject buffObj;
    private GameObject deBuffObj;
    public override void Init()
    {
        base.Init();

        buffObj = Resources.Load<GameObject>("UI/Popup/Boss/Buff");
        deBuffObj = Resources.Load<GameObject>("UI/Popup/Boss/DeBuff");

        Bind<Image>(typeof(UI_Boss_Objects));
        Bind<TextMeshProUGUI>(typeof(UI_Boss_Objects));
        Bind<GameObject>(typeof(UI_Boss_Objects));

        // enum 인덱스로 실제 변수에 할당
        hpFill = Get<Image>((int)UI_Boss_Objects.HPFill);
        buffDebuffContainer = Get<GameObject>((int)UI_Boss_Objects.BuffDebuffContainer);
        bossNameText = Get<TextMeshProUGUI>((int)UI_Boss_Objects.BossNameText);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            AddBuff();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            AddDeBuff();
        }
    }
    public void SetBossName(string name)
    {
        bossNameText.text = name;
    }

    public void SetHPFill(SC_MONSTER_CONDITION packet)
    {
        float currentHp = packet.CurrentHp;
        float maxHp = packet.MaxHp;
        hpFill.fillAmount = currentHp / maxHp;
    }

    public void AddBuff()
    {
        GameObject obj = Instantiate(buffObj, buffDebuffContainer.transform);
    }

    public void AddDeBuff()
    {
        GameObject obj = Instantiate(deBuffObj, buffDebuffContainer.transform);
    }

    public void SetBossNameText(string name)
    {
        bossNameText.text = name;
    }
}


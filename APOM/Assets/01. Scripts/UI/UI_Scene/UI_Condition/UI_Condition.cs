using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ConditionObject
{
    HPFill,
}

public class UI_Condition : UI_Base
{
    public Player player;

    private Image hpFill;
    public override void Init()
    {
        Bind<Image>(typeof(ConditionObject));
        hpFill = Get<Image>((int)(ConditionObject.HPFill));
    }

    public void SetFill()
    {
        hpFill.fillAmount = player.Stat.currentHp / player.Stat.maxHp;
    }
}

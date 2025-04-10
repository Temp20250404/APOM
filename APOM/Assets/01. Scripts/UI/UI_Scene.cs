using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scene : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, false);

        UI_Scene[] children = GetComponentsInChildren<UI_Scene>(true);

        foreach (var ui in children)
        {
            if (ui != this)
                ui.Init();
        }
    }
}

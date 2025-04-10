using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Follow : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void CloseFollowUI()
    {
        Managers.UI.CloseFollowUI(this);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PopupType
{
    Login,
    Inventory,
}
public class UI_Popup : UI_Base
{
    public PopupType popupType;

    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}

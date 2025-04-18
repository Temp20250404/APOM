using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Main : UI_Scene, IChat
{
    public UI_Chat chat;
    public UI_MiniMap minimap;
    public UI_Condition condition;
    public UI_QuickSlot quickSlot;

    public override void Init()
    {
        // 순서대로 UI를 생성한다. (Cansvas SortOrder 순서)
        // 생성후 바로 Init()을 호출한다.
        minimap = Managers.UI.ShowSceneChildUI<UI_MiniMap>(transform);
        condition = Managers.UI.ShowSceneChildUI<UI_Condition>(transform);
        quickSlot = Managers.UI.ShowSceneChildUI<UI_QuickSlot>(transform);
        chat = Managers.UI.ShowSceneChildUI<UI_Chat>(transform);
    }

    public void ReceiveChat(string message, uint channel)
    {
        chat.ServerByChat(message, channel);
    }
}

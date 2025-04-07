using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class UI_PopupTest : UI_Popup
{
    enum Buttons
    {
        Button, //오브젝트의 이름
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        // 버튼 클릭 시 닫기
        GetButton((int)Buttons.Button).onClick.AddListener(() => 
        { 
            Managers.UI.ClosePopupUI();
        });
    }
}

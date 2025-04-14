using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum IDFindType
{
    EMail_Input,
    ID_Find_Button,
    FindResult_Text,
    Exit_Button
}
public class UI_IDFind : UI_Popup
{
    private TMP_InputField emailInputField;
    private Button idFindButton;
    private TextMeshProUGUI findResultText;
    private Button exitButton;

    public override void Init()
    {
        base.Init();

        popupType = PopupType.Login;

        // enum 이름 기준으로 자동 바인딩
        Bind<InputField>(typeof(IDFindType));      // EMail_Input
        Bind<Button>(typeof(IDFindType));          // 모든 버튼류
        Bind<TextMeshProUGUI>(typeof(IDFindType)); // FindResult_Text

        // enum 인덱스로 실제 변수에 할당
        emailInputField = Get<TMP_InputField>((int)IDFindType.EMail_Input);
        idFindButton = Get<Button>((int)IDFindType.ID_Find_Button);
        findResultText = Get<TextMeshProUGUI>((int)IDFindType.FindResult_Text);
        exitButton = Get<Button>((int)IDFindType.Exit_Button);

        // 버튼 이벤트 연결
        idFindButton.onClick.AddListener(OnClickIDFind);
        exitButton.onClick.AddListener(ClosePopupUI);
    }

    public void OnClickIDFind()
    {
        Debug.Log("ID Find Button Clicked");
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Login>();
    }
}

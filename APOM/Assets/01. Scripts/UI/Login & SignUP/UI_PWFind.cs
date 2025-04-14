using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PWFindType
{
    ID_Input,
    EMail_Input,
    PW_Find_Button,
    FindResult_Text,
    Exit_Button
}

public class UI_PWFind : UI_Popup
{
    private TMP_InputField idInputField;
    private TMP_InputField emailInputField;
    private Button pwFindButton;
    private TextMeshProUGUI findResultText;
    private Button exitButton;

    public override void Init()
    {
        base.Init();

        popupType = PopupType.Login;

        // enum 이름 기준으로 자동 바인딩
        Bind<InputField>(typeof(PWFindType));      // EMail_Input
        Bind<Button>(typeof(PWFindType));          // 모든 버튼류
        Bind<TextMeshProUGUI>(typeof(PWFindType)); // FindResult_Text

        // enum 인덱스로 실제 변수에 할당
        emailInputField = Get<TMP_InputField>((int)PWFindType.EMail_Input);
        pwFindButton = Get<Button>((int)PWFindType.PW_Find_Button);
        findResultText = Get<TextMeshProUGUI>((int)PWFindType.FindResult_Text);
        exitButton = Get<Button>((int)PWFindType.Exit_Button);

        // 버튼 이벤트 연결
        pwFindButton.onClick.AddListener(OnClickIDFind);
        exitButton.onClick.AddListener(ClosePopupUI);
    }

    public void OnClickIDFind()
    {
        Debug.Log("PW Find Button Clicked");
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Login>();
    }
}

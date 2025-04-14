using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum SignUpType
{
    ID_Input,
    EMail_Input,
    PW_Input,
    PW_Check_Input,
    SignUp_Button,
    SignUp_Error_Text,
    Exit_Button
}

public class UI_SignUp : UI_Popup
{

    private TMP_InputField idInputField;
    private TMP_InputField emailInputField;
    private TMP_InputField pwInputField;
    private TMP_InputField pwCheckInputField;

    private Button signUpButton;
    private TextMeshProUGUI signUpErrorText;

    private Button exitButton;

    public override void Init()
    {
        base.Init();

        popupType = PopupType.Login;

        // enum 이름 기준으로 자동 바인딩
        Bind<TextMeshProUGUI>(typeof(SignUpType));   // Login_Error_Text
        Bind<TMP_InputField>(typeof(SignUpType));    // ID_Input, PW_Input
        Bind<Button>(typeof(SignUpType));            // 모든 버튼류

        // enum 인덱스로 실제 변수에 할당
        idInputField = Get<TMP_InputField>((int)SignUpType.ID_Input);
        emailInputField = Get<TMP_InputField>((int)SignUpType.EMail_Input);
        pwInputField = Get<TMP_InputField>((int)SignUpType.PW_Input);
        pwCheckInputField = Get<TMP_InputField>((int)SignUpType.PW_Check_Input);
        signUpButton = Get<Button>((int)SignUpType.SignUp_Button);
        signUpErrorText = Get<TextMeshProUGUI>((int)SignUpType.SignUp_Error_Text);
        exitButton = Get<Button>((int)SignUpType.Exit_Button);

        // 버튼 이벤트 연결
        signUpButton.onClick.AddListener(OnClickSignUp);
        exitButton.onClick.AddListener(ClosePopupUI);
    }

    public void OnClickSignUp()
    {
        Debug.Log("Sign Up Button Clicked");
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Login>();
    }
}

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
    SignUp_Result_Text,
    Exit_Button
}

public class UI_SignUp : UI_Popup
{

    private TMP_InputField idInputField;
    private TMP_InputField emailInputField;
    private TMP_InputField pwInputField;
    private TMP_InputField pwCheckInputField;

    private Button signUpButton;
    private TextMeshProUGUI signUpResultText;

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
        signUpResultText = Get<TextMeshProUGUI>((int)SignUpType.SignUp_Result_Text);
        exitButton = Get<Button>((int)SignUpType.Exit_Button);

        // 버튼 이벤트 연결
        signUpButton.onClick.AddListener(OnClickSignUp);
        exitButton.onClick.AddListener(ClosePopupUI);

        TextEmpty();
    }

    public void OnClickSignUp()
    {
        string id = idInputField.text;
        string pw = pwInputField.text;
        string email = emailInputField.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            signUpResultText.text = "원하는 ID와 비밀번호를 입력하세요.";
            return;
        }

        if (IsIDDuplicated(id))
        {
            signUpResultText.text = "이미 존재하는 ID입니다.";
            return;
        }

        if (pwInputField.text != pwCheckInputField.text)
        {
            signUpResultText.text = "비밀번호가 일치하지 않습니다.";
            return;
        }

        int count = PlayerPrefs.GetInt("IDCount", 0);
        PlayerPrefs.SetString($"User_{count}_ID", id);
        PlayerPrefs.SetString($"User_{count}_PW", pw);
        PlayerPrefs.SetString($"User_{count}_Email", email);
        PlayerPrefs.SetInt("IDCount", count + 1);
        PlayerPrefs.Save();

        Debug.Log("회원가입 성공!");
        signUpResultText.text = "회원가입 성공!";

        TextEmpty();
    }

    private bool IsIDDuplicated(string id)
    {
        int count = PlayerPrefs.GetInt("IDCount", 0);

        for(int i = 0; i < count; i++)
        {
            string existingID = PlayerPrefs.GetString($"User_{i}_ID", "");

            if (existingID == id)
            {
                return true;
            }
        }
        return false;
    }

    public void TextEmpty()
    {
        idInputField.text = "";
        emailInputField.text = "";
        pwInputField.text = "";
        pwCheckInputField.text = "";
    }
    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Login>();
    }
}

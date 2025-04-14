using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum PWFindType
{
    ID_Input,
    Email_Input,
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
        Bind<TMP_InputField>(typeof(PWFindType));      // EMail_Input
        Bind<Button>(typeof(PWFindType));          // 모든 버튼류
        Bind<TextMeshProUGUI>(typeof(PWFindType)); // FindResult_Text

        // enum 인덱스로 실제 변수에 할당
        idInputField = Get<TMP_InputField>((int)PWFindType.ID_Input);
        emailInputField = Get<TMP_InputField>((int)PWFindType.Email_Input);
        pwFindButton = Get<Button>((int)PWFindType.PW_Find_Button);
        findResultText = Get<TextMeshProUGUI>((int)PWFindType.FindResult_Text);
        exitButton = Get<Button>((int)PWFindType.Exit_Button);

        // 버튼 이벤트 연결
        pwFindButton.onClick.AddListener(OnClickPWFind);
        exitButton.onClick.AddListener(ClosePopupUI);

        TextEmpty();
    }

    public void OnClickPWFind()
    {
        string id = idInputField.text;
        string email = emailInputField.text;
        string pw = IsPWFind(id, email);

        if (!string.IsNullOrEmpty(pw))
        {
            findResultText.text = $"PW: {pw}";
        }
        else
        {
            findResultText.text = "ID가 없습니다, \n ID와 Email을 확인해주세요.";
        }

        if (email == string.Empty || id == string.Empty)
        {
            findResultText.text = "ID와 Email을 입력해 주세요";
        }

        TextEmpty();
    }

    private string IsPWFind(string id, string email)
    {
        int count = PlayerPrefs.GetInt("IDCount", 0);

        for (int i = 0; i < count; i++)
        {
            string existingEmail = PlayerPrefs.GetString($"User_{i}_Email", "");
            string existingID = PlayerPrefs.GetString($"User_{i}_ID", "");

            if (existingEmail == email && existingID == id)
            {
                return PlayerPrefs.GetString($"User_{i}_PW", ""); ;
            }
        }
        return null;
    }

    public void TextEmpty()
    {
        idInputField.text = "";
        emailInputField.text = "";
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Login>();
    }
}

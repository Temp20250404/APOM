using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Game;

public enum IDFindType
{
    Email_Input,
    ID_Find_Button,
    FindResult_Text,
    Exit_Button
}
public class UI_IDFind : UI_Popup
{
    private TMP_InputField emailInputField;
    private Button idFindButton;
    public TextMeshProUGUI findResultText;
    private Button exitButton;

    public override void Init()
    {
        base.Init();

        popupType = PopupType.Login;

        // enum �̸� �������� �ڵ� ���ε�
        Bind<TMP_InputField>(typeof(IDFindType));      // EMail_Input
        Bind<Button>(typeof(IDFindType));          // ��� ��ư��
        Bind<TextMeshProUGUI>(typeof(IDFindType)); // FindResult_Text

        // enum �ε����� ���� ������ �Ҵ�
        emailInputField = Get<TMP_InputField>((int)IDFindType.Email_Input);
        idFindButton = Get<Button>((int)IDFindType.ID_Find_Button);
        findResultText = Get<TextMeshProUGUI>((int)IDFindType.FindResult_Text);
        exitButton = Get<Button>((int)IDFindType.Exit_Button);

        // ��ư �̺�Ʈ ����
        idFindButton.onClick.AddListener(OnClickIDFind);
        exitButton.onClick.AddListener(ClosePopupUI);

        TextEmpty();
    }

    public void OnClickIDFind()
    {
        string email = emailInputField.text;
        //string id = IsIDFind(email);

        if (email == string.Empty)
        {
            findResultText.text = "Email �Է��� �ּ���";
            return;
        }

        CS_FIND_ID_REQUEST ptk = new CS_FIND_ID_REQUEST();
        ptk.Email = email;

        Managers.Network.Send(ptk);

        //if (!string.IsNullOrEmpty(id))
        //{
        //    findResultText.text = $"ID: {id}";
        //}
        //else
        //{
        //    findResultText.text = "��ϵ� ID�� �����ϴ�.";
        //}

        TextEmpty();
    }

    //private string IsIDFind(string email)
    //{
    //    int count = PlayerPrefs.GetInt("IDCount", 0);

    //    for (int i = 0; i < count; i++)
    //    {
    //        string existingEmail = PlayerPrefs.GetString($"User_{i}_Email", "");

    //        if (existingEmail == email)
    //        {
    //            return PlayerPrefs.GetString($"User_{i}_ID", ""); ;
    //        }
    //    }
    //    return null;
    //}

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_Login>();
    }

    public void TextEmpty()
    {
        emailInputField.text = "";
    }
}

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

        // enum �̸� �������� �ڵ� ���ε�
        Bind<InputField>(typeof(IDFindType));      // EMail_Input
        Bind<Button>(typeof(IDFindType));          // ��� ��ư��
        Bind<TextMeshProUGUI>(typeof(IDFindType)); // FindResult_Text

        // enum �ε����� ���� ������ �Ҵ�
        emailInputField = Get<TMP_InputField>((int)IDFindType.EMail_Input);
        idFindButton = Get<Button>((int)IDFindType.ID_Find_Button);
        findResultText = Get<TextMeshProUGUI>((int)IDFindType.FindResult_Text);
        exitButton = Get<Button>((int)IDFindType.Exit_Button);

        // ��ư �̺�Ʈ ����
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

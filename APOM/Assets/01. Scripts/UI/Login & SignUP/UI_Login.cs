using Game;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum LoginType
{
    Login_Result_Text,
    ID_Input,
    PW_Input,
    Login_Button,
    ID_Check_Toggle,
    ID_Find_Button,
    PW_Find_Button,
    SignUp_Button,
    CustomerService_Button
}
public class UI_Login : UI_Popup
{
    public TextMeshProUGUI loginResultText;
    private TMP_InputField idInputField;
    private TMP_InputField pwInputField;
    private Button loginButton;
    private Toggle idCheckToggle;

    private Button idFindButton;
    private Button pwFindButton;
    private Button signUpButton;
    private Button customerServiceButton;

    public override void Init()
    {
        base.Init();

        popupType = PopupType.Login;

        // enum 이름 기준으로 자동 바인딩
        Bind<TextMeshProUGUI>(typeof(LoginType));   // Login_Error_Text
        Bind<TMP_InputField>(typeof(LoginType));    // ID_Input, PW_Input
        Bind<Button>(typeof(LoginType));            // 모든 버튼류
        Bind<Toggle>(typeof(LoginType));            // ID_Check_Toggle

        // enum 인덱스로 실제 변수에 할당
        loginResultText = Get<TextMeshProUGUI>((int)LoginType.Login_Result_Text);
        idInputField = Get<TMP_InputField>((int)LoginType.ID_Input);
        pwInputField = Get<TMP_InputField>((int)LoginType.PW_Input);
        loginButton = Get<Button>((int)LoginType.Login_Button);
        idCheckToggle = Get<Toggle>((int)LoginType.ID_Check_Toggle);
        idFindButton = Get<Button>((int)LoginType.ID_Find_Button);
        pwFindButton = Get<Button>((int)LoginType.PW_Find_Button);
        signUpButton = Get<Button>((int)LoginType.SignUp_Button);
        customerServiceButton = Get<Button>((int)LoginType.CustomerService_Button);

        // 버튼 이벤트 연결
        loginButton.onClick.AddListener(OnClickLogin);
        signUpButton.onClick.AddListener(OnClickSignUp);
        customerServiceButton.onClick.AddListener(OnClickCustomerService);
        idFindButton.onClick.AddListener(OnClickIDFind);
        pwFindButton.onClick.AddListener(OnClickPWFind);

        GetID();
    }

    public void OnClickLogin()
    {
        string id = idInputField.text;
        string pw = pwInputField.text;

        if(string.IsNullOrEmpty(id) || string.IsNullOrEmpty(pw))
        {
            loginResultText.text = "ID 또는 비밀번호를 입력해 주세요";
            return;
        }

        CS_LOGIN_REQUEST ptk = new CS_LOGIN_REQUEST();
        ptk.Id = id;
        ptk.Password = pw;
        Managers.Network.Send(ptk);

        CheckToggle(id);
        //loginResultText.text = "ID 또는 비밀번호가 틀렸습니다.";
        //idInputField.text = "";
        //pwInputField.text = "";

        //int count = PlayerPrefs.GetInt("IDCount", 0);

        //for(int i = 0; i < count; i++)
        //{
        //    string savedID = PlayerPrefs.GetString($"User_{i}_ID");
        //    string savedPW = PlayerPrefs.GetString($"User_{i}_PW");

        //    if (id == savedID && pw == savedPW)
        //    {
        //        loginResultText.text = "로그인 성공";
        //        SceneManager.LoadScene("HMJScene");

        //        TextEmpty(id);
        //        return;
        //    }
        //}
    }

    //public void TextEmpty(string id)
    //{
    //    if (idCheckToggle.isOn)
    //    {
    //        PlayerPrefs.SetInt("SaveIDToggle", 1);
    //        PlayerPrefs.SetString("SavedID", id);
    //        PlayerPrefs.Save();
    //        pwInputField.text = "";
    //    }
    //    else
    //    {
    //        CheckToggle();
    //        idInputField.text = "";
    //        pwInputField.text = "";
    //    }
    //}

    public void CheckToggle(string id)
    {
        if (idCheckToggle.isOn)
        {
            PlayerPrefs.SetInt("SaveIDToggle", 1);
            PlayerPrefs.SetString("SavedID", id);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.DeleteKey("SavedID");
            PlayerPrefs.DeleteKey("SaveIDToggle");
        }

        idInputField.text = "";
        pwInputField.text = "";
    }
    public void GetID()
    {
        bool isToggleOn = PlayerPrefs.GetInt("SaveIDToggle", 0) == 1;
        idCheckToggle.isOn = isToggleOn;

        if (isToggleOn)
        {
            idInputField.text = PlayerPrefs.GetString("SavedID", "");
            pwInputField.text = "";
        }
        else
        {
            idInputField.text = "";
            pwInputField.text = "";
        }
    }

    public void OnClickCustomerService()
    {
        ClosePopupUI();
        Debug.Log("Customer Service Button Clicked");
    }
    public void OnClickSignUp()
    {
        ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_SignUp>();
    }

    public void OnClickIDFind()
    {
        ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_IDFind>();
    }

    public void OnClickPWFind()
    {
        ClosePopupUI();
        Managers.UI.ShowPopupUI<UI_PWFind>();
    }


}

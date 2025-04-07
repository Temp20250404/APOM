using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ChatType
{
    Normal = 0,
    Party,
    Whisper,
    All,
    Count
}

//public enum ChatViewType
//{
//    Normal,
//    Party,
//    Whisper,
//    All,
//    Count
//}
public class ChatController : MonoBehaviour
{
    [SerializeField]
    private GameObject textChatPrefab; // 대화 텍스트 프리팹
    [SerializeField]
    private Transform parentContent; // 대화가 출력되는 ScrollView의 Content
    [SerializeField]
    private TMP_InputField inputField; // 대화 입력창

    [SerializeField]
    private TextMeshProUGUI chatTypeText; // 대화 속성 변경 버튼의 텍스트
    [SerializeField]
    private TextMeshProUGUI textInput; // inputField의 텍스트

    private ChatType currentInputType; // 현재 대화 입력 속성
    private Color currentTextColor; // 대화 입력 속성의 색상

    private List<ChatCell> chatList; // 대화창에 출력되는 모든 대화를 보관하는 리스트
    private ChatType currentViewType; // 현재 대화 보기 속성

    private string ID = "Player"; // 대화 ID

    private void Awake()
    {
        chatList = new List<ChatCell>();

        currentInputType = ChatType.Normal;
        currentTextColor = Color.red;
        textInput.color = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            inputField.ActivateInputField(); // 대화 입력창 활성화
        }

        if(Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused == true)
        {
            SetCurrentInputType();
        }
    }

    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {

            UpdateChat();
        }
    }

    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return;
        
        GameObject clone = Instantiate(textChatPrefab, parentContent);
        ChatCell chatCell = clone.GetComponent<ChatCell>();

        chatCell.Setup(currentInputType, currentTextColor, SetupText());

        inputField.text = ""; // 대화 입력창 초기화

        chatList.Add(chatCell); // 대화 리스트에 추가
    }

    private Color ChatTypeToColor(ChatType type)
    {
        Color[] colors = new Color[] { Color.red, Color.blue, Color.magenta };

        return colors[(int)type];
    }

    private string ChatTypeToName(ChatType type)
    {
        string[] names = new string[] { "Normal", "Party", "Whisper" };
        return names[(int)type];
    }

    private string SetupText()
    {
        string time = DateTime.Now.ToString("HH:mm");
        string text = $"[{time}] [{currentInputType}] [{ID}] : {inputField.text}";

        return text;
    }

    public void SetCurrentInputType()
    {
        currentInputType = (int)currentInputType < (int)ChatType.Count - 2? currentInputType + 1 : 0;
        chatTypeText.text = ChatTypeToName(currentInputType);
        currentTextColor = ChatTypeToColor(currentInputType);
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    public void SetCurrentViewType(int newType)
    {
        currentViewType = (ChatType)newType;

        if(currentViewType == ChatType.All)
        {
            for(int i = 0; i < chatList.Count; ++ i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        else
        {
            for(int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }
}

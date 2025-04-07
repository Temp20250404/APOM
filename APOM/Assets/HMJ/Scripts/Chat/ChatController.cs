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
    private GameObject textChatPrefab; // ��ȭ �ؽ�Ʈ ������
    [SerializeField]
    private Transform parentContent; // ��ȭ�� ��µǴ� ScrollView�� Content
    [SerializeField]
    private TMP_InputField inputField; // ��ȭ �Է�â

    [SerializeField]
    private TextMeshProUGUI chatTypeText; // ��ȭ �Ӽ� ���� ��ư�� �ؽ�Ʈ
    [SerializeField]
    private TextMeshProUGUI textInput; // inputField�� �ؽ�Ʈ

    private ChatType currentInputType; // ���� ��ȭ �Է� �Ӽ�
    private Color currentTextColor; // ��ȭ �Է� �Ӽ��� ����

    private List<ChatCell> chatList; // ��ȭâ�� ��µǴ� ��� ��ȭ�� �����ϴ� ����Ʈ
    private ChatType currentViewType; // ���� ��ȭ ���� �Ӽ�

    private string ID = "Player"; // ��ȭ ID

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
            inputField.ActivateInputField(); // ��ȭ �Է�â Ȱ��ȭ
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

        inputField.text = ""; // ��ȭ �Է�â �ʱ�ȭ

        chatList.Add(chatCell); // ��ȭ ����Ʈ�� �߰�
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

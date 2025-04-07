using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ChatType
{
    Normal = 0,   // �Ϲ� ��ȭ
    Party,        // ��Ƽ ��ȭ
    Whisper,      // �ӼӸ� ��ȭ
    All,          // ��� ��ȭ (�����)
    Count         // ChatType �׸��� ��
}

public class ChatController : MonoBehaviour
{
    [SerializeField]
    private GameObject textChatPrefab; // ��ȭ �ؽ�Ʈ ������ (UI���� ������ �ؽ�Ʈ ������Ʈ)
    [SerializeField]
    private Transform parentContent; // ��ȭ�� ��µ� ScrollView�� Content ���� (��ũ�� ���� ����Ʈ)
    [SerializeField]
    private TMP_InputField inputField; // ��ȭ �Է�â (������ �ؽ�Ʈ�� �Է��ϴ� ��)

    [SerializeField]
    private TextMeshProUGUI chatTypeText; // ���� ��ȭ �Ӽ� ���� ��ư�� ǥ�õ� �ؽ�Ʈ (��: Normal, Party, Whisper)
    [SerializeField]
    private TextMeshProUGUI textInput; // ��ȭ �Է�â�� �ؽ�Ʈ ����

    private ChatType currentInputType; // ���� ��ȭ �Է� �Ӽ� (��: Normal, Party, Whisper)
    private Color currentTextColor; // ���� ��ȭ �Ӽ��� ���� �ؽ�Ʈ ����

    private List<ChatCell> chatList; // ��ȭâ�� ��µ� ��� ��ȭ�� �����ϴ� ����Ʈ
    private ChatType currentViewType; // ���� ���õ� ��ȭ ���� �Ӽ�

    private string ID = "Player"; // ��ȭ���� ����� ID (��: ������ �̸�)

    private void Awake()
    {
        chatList = new List<ChatCell>(); // ��ȭ ������ ������ ����Ʈ �ʱ�ȭ

        currentInputType = ChatType.Normal; // �⺻ ��ȭ ���� ���� (Normal)
        currentTextColor = Color.red; // �⺻ �ؽ�Ʈ ���� ���� (red)
        textInput.color = Color.red; // ��ȭ �Է�â�� �ؽ�Ʈ ���� ����
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            inputField.ActivateInputField(); // ��ȭ �Է�â Ȱ��ȭ
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused == true)
        {
            SetCurrentInputType(); // Tab Ű ������ �� ��ȭ �Է� Ÿ�� ��ȯ
        }
    }

    // EndEditEvent�� ȣ��� �� ��ȭ �ؽ�Ʈ �Է� ó��
    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // EnterŰ�� ������ ��ȭ ó��
        {
            UpdateChat();
        }
    }

    // ��ȭ ������Ʈ �޼���
    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return; // �Էµ� �ؽ�Ʈ�� ������ ó������ ����

        GameObject clone = Instantiate(textChatPrefab, parentContent); // �ؽ�Ʈ �������� �θ� Content�� �ν��Ͻ�ȭ
        ChatCell chatCell = clone.GetComponent<ChatCell>(); // ChatCell ������Ʈ�� ������

        chatCell.Setup(currentInputType, currentTextColor, SetupText()); // ���� ��ȭ ���� ����

        inputField.text = ""; // ��ȭ �Է�â �ʱ�ȭ

        chatList.Add(chatCell); // ���� ������ ��ȭ ���� ����Ʈ�� �߰�
    }

    // ChatType�� �´� �ؽ�Ʈ ������ ��ȯ�ϴ� �޼���
    private Color ChatTypeToColor(ChatType type)
    {
        Color[] colors = new Color[] { Color.red, Color.blue, Color.magenta }; // �� ChatType�� �����ϴ� ���� �迭
        return colors[(int)type]; // �迭 �ε����� ���� ���� ��ȯ
    }

    // ChatType�� �´� �ؽ�Ʈ �̸��� ��ȯ�ϴ� �޼���
    private string ChatTypeToName(ChatType type)
    {
        string[] names = new string[] { "Normal", "Party", "Whisper" }; // �� ChatType�� �����ϴ� �̸� �迭
        return names[(int)type]; // �迭 �ε����� ���� �̸� ��ȯ
    }

    // ��ȭ �ؽ�Ʈ�� �ð��� �߰��ϰ� �����ϴ� �޼���
    private string SetupText()
    {
        string time = DateTime.Now.ToString("HH:mm"); // ���� �ð� �������� (HH:mm ����)
        string text = $"[{time}] [{currentInputType}] [{ID}] : {inputField.text}"; // ���� �ؽ�Ʈ ����

        return text;
    }

    // ��ȭ �Է� Ÿ�� ���� �޼���
    public void SetCurrentInputType()
    {
        // �Է� Ÿ���� ���������� ���� (Normal -> Party -> Whisper -> Normal...)
        currentInputType = (int)currentInputType < (int)ChatType.Count - 2 ? currentInputType + 1 : 0;
        chatTypeText.text = ChatTypeToName(currentInputType); // ��ư �ؽ�Ʈ�� ���� Ÿ�� �̸� ǥ��
        currentTextColor = ChatTypeToColor(currentInputType); // ���� Ÿ�Կ� �´� �ؽ�Ʈ ���� ����
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor; // �Է�â �ؽ�Ʈ ���� ����
    }

    // ��ȭ ���� Ÿ�� ���� �޼���
    public void SetCurrentViewType(int newType)
    {
        currentViewType = (ChatType)newType;

        if (currentViewType == ChatType.All) // 'All'�� ������ ��� ��� ��ȭ ���� Ȱ��ȭ
        {
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        else // �� �� Ÿ���� �ش� Ÿ�Կ� �´� ��ȭ�� Ȱ��ȭ
        {
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }
}

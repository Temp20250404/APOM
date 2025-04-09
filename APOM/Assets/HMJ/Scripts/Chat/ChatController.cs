using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public enum ChatType
{
    Normal = 0,   // �Ϲ� ��ȭ
    Party,        // ��Ƽ ��ȭ
    Whisper,      // �ӼӸ� ��ȭ
    All,          // ��� ��ȭ (�����)
    Count         // ChatType �׸��� ��
}

public class ChatController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
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

    private RectTransform rectTransform; // RectTransform ������Ʈ (UI ����� ��ġ�� ũ�⸦ �����ϴ� �� ���)
    private Vector2 originalSize; // ���� ũ�� ���� (UI ����� ũ�⸦ �����ϱ� ���� ���)
    private Vector2 originalMousePosition; // ���� ���콺 ��ġ ���� (�巡�� ���� ����)
    private Vector2 currentSize; // ���� ũ�� ���� (UI ����� ũ�⸦ �����ϱ� ���� ���)

    private float minChatSize = 20f; // �ּ� �ʺ� (ä�� �ؽ�Ʈ�� �ּ� ũ��)
    private float maxChatSize = 40f; // �ִ� �ʺ� (ä�� �ؽ�Ʈ�� �ִ� ũ��)

    private bool isToggleCollapse; // ��ȭâ ����/��ġ�� ���� (true: ����, false: ������)

    private void Awake()
    {
        chatList = new List<ChatCell>(); // ��ȭ ������ ������ ����Ʈ �ʱ�ȭ

        currentInputType = ChatType.Normal; // �⺻ ��ȭ ���� ���� (Normal)
        currentTextColor = Color.red; // �⺻ �ؽ�Ʈ ���� ���� (red)
        textInput.color = Color.red; // ��ȭ �Է�â�� �ؽ�Ʈ ���� ����
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform ������Ʈ ��������
        originalSize = rectTransform.sizeDelta; // ���� ũ�� ����
        currentSize = originalSize; // ���� ũ�� �ʱ�ȭ
        Debug.Log(originalSize);
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
        chatCell.GetComponent<TextMeshProUGUI>().fontSize = NewChatSize(); // ��Ʈ ũ�� ����

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

    public void ToggleCollapseChatWindow() // ��ȭâ ����/��ġ�� �޼���
    {
        isToggleCollapse = !isToggleCollapse; // ���� ���� ����
        bool isOn = rectTransform.sizeDelta.y < currentSize.y; // ���� ���� Ȯ�� (������ �ִ��� ���� �ִ���)
        rectTransform.sizeDelta = isOn ? currentSize : new Vector2(originalSize.x, 200); // ���� ũ��� ����
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isToggleCollapse) return; // ��ȭâ�� ���� �ִ� ��� �巡�� ���� ó�� �� ��
        currentSize = rectTransform.sizeDelta; // �巡�� ���� �� ���� ũ�� ����
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isToggleCollapse) return; // ��ȭâ�� ���� �ִ� ��� �巡�� ó�� �� ��
        Vector2 delta = eventData.position - originalMousePosition; // �巡���� �Ÿ� ���

        rectTransform.sizeDelta = new Vector2(Mathf.Clamp(currentSize.x + delta.x, 500, 1600),
            Mathf.Clamp(currentSize.y + delta.y, 200, 800)); // ũ�� ����

        AdjustFontSize(delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isToggleCollapse) return; // ��ȭâ�� ���� �ִ� ��� �巡�� ���� ó�� �� ��
        originalMousePosition = eventData.position; // �巡�� ���� ���� ����
    }

    private void AdjustFontSize(Vector2 delta)
    {
        foreach (ChatCell chatCell in chatList)
        {
            var cell = chatCell.GetComponent<TextMeshProUGUI>();
            cell.fontSize = NewChatSize(); // �� ��ȭ ���� ��Ʈ ũ�� ����
            cell.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(currentSize.x + delta.x -20, 480, 1600), cell.rectTransform.sizeDelta.y); // �� ũ�� ����
        }   
    }

    private float NewChatSize()
    {
        // ���� ũ�⿡ ���� ��Ʈ ũ�� ����
        float widthRatio = rectTransform.sizeDelta.x / originalSize.x;
        float heightRatio = rectTransform.sizeDelta.y / originalSize.y;
        float newChatSize = Mathf.Clamp(Mathf.Max(widthRatio * minChatSize, heightRatio * minChatSize), minChatSize, maxChatSize);

        return newChatSize; // ������ ��Ʈ ũ�� ��ȯ
    }
}

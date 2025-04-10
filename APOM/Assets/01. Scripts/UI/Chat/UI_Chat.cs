using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 바인딩할 UI 오브젝트 이름 정의 (이름은 Hierarchy 오브젝트 이름과 1:1 매칭돼야 함)
/// </summary>
public enum ChatObjects
{
    ParentContent,     // 채팅 메시지를 출력할 ScrollView의 Content 영역
    InputField,        // 채팅 입력 필드 (TMP_InputField)
    ChatTypeText,      // 채팅 타입 (Normal, Party 등) 표시 텍스트
    TextInput          // 채팅 입력 텍스트 색상용 텍스트 오브젝트
}

/// <summary>
/// 채팅 타입 (입력/필터링 시 구분)
/// </summary>
public enum ChatType
{
    Normal = 0,
    Party,
    Whisper,
    All,     // 전체 보기 전용
    Count    // enum 길이 파악용
}

public class UI_Chat : UI_Scene, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // 프리팹: 채팅 메시지를 하나씩 출력할 오브젝트 (SubItem/ChatCell)
    private GameObject ChatCell;

    // UI 요소들
    private Transform parentContent;
    private TMP_InputField inputField;
    private TextMeshProUGUI chatTypeText;
    private TextMeshProUGUI textInput;

    // 현재 채팅 타입과 색상
    private ChatType currentInputType = ChatType.Normal;
    private Color currentTextColor;

    // 현재까지 출력된 채팅 메시지 리스트
    private List<ChatCell> chatList = new();
    private ChatType currentViewType = ChatType.All;

    // 임시 플레이어 ID
    private string ID = "Player";

    // 채팅창 리사이징 및 드래그 관련 상태 변수
    private RectTransform rectTransform;
    private Vector2 originalSize;
    private Vector2 originalMousePosition;
    private Vector2 currentSize;

    // 채팅창 사이즈 및 폰트 제한
    private const float MinFontSize = 20f;
    private const float MaxFontSize = 40f;
    private const float MinWidth = 500f;
    private const float MaxWidth = 1600f;
    private const float MinHeight = 200f;
    private const float MaxHeight = 800f;

    // 대화창 접힘/펼침 상태
    private bool isToggleCollapse;

    // 채팅 타입별 색상 매핑
    private static readonly Dictionary<ChatType, Color> ChatColors = new()
    {
        { ChatType.Normal, Color.red },
        { ChatType.Party, Color.blue },
        { ChatType.Whisper, Color.magenta }
    };

    // 채팅 타입별 표시 텍스트
    private static readonly Dictionary<ChatType, string> ChatNames = new()
    {
        { ChatType.Normal, "Normal" },
        { ChatType.Party, "Party" },
        { ChatType.Whisper, "Whisper" }
    };

    /// <summary>
    /// UI 초기화: 바인딩 및 필드 초기화
    /// </summary>
    public override void Init()
    {
        Bind<GameObject>(typeof(ChatObjects)); // GameObject만 바인딩하고, 컴포넌트는 직접 꺼냄

        ChatCell = Managers.Resource.Load<GameObject>("UI/Scene/ChatCell"); // 채팅 셀 프리팹 로드

        // 각 오브젝트의 컴포넌트 추출 (null-safe)
        parentContent = GetObject((int)ChatObjects.ParentContent)?.transform;
        chatTypeText = GetObject((int)ChatObjects.ChatTypeText)?.GetComponent<TextMeshProUGUI>();
        inputField = GetObject((int)ChatObjects.InputField)?.GetComponent<TMP_InputField>();
        textInput = GetObject((int)ChatObjects.TextInput)?.GetComponent<TextMeshProUGUI>();

        currentInputType = ChatType.Normal;
        SetChatInputType(currentInputType); // 기본 채팅 타입 설정

        rectTransform = GetComponent<RectTransform>();
        originalSize = rectTransform.sizeDelta;
        currentSize = originalSize;
    }

    /// <summary>
    /// 키 입력 핸들링 (엔터, 탭)
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !inputField.isFocused)
            inputField.ActivateInputField(); // 포커스 없을 때 엔터 → 입력창 활성화

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused)
            SetNextInputType(); // 포커스 있을 때 탭 → 입력 채팅 타입 순환
    }

    /// <summary>
    /// 엔터 입력 후 채팅 전송 처리
    /// </summary>
    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            UpdateChat();
    }

    /// <summary>
    /// 채팅 텍스트를 인스턴스화하여 출력
    /// </summary>
    public void UpdateChat()
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return;

        var chatCell = Instantiate(ChatCell, parentContent).GetComponent<ChatCell>();
        chatCell.Setup(currentInputType, currentTextColor, ComposeChatText());
        chatCell.GetComponent<TextMeshProUGUI>().fontSize = CalculateChatFontSize();

        chatList.Add(chatCell);
        inputField.text = string.Empty;
    }

    /// <summary>
    /// 채팅 타입에 따른 색상/텍스트 설정
    /// </summary>
    private void SetChatInputType(ChatType type)
    {
        currentInputType = type;
        currentTextColor = ChatColors[type];
        chatTypeText.text = ChatNames[type];
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor;
    }

    /// <summary>
    /// 채팅 타입을 순환시킴 (탭 키)
    /// </summary>
    private void SetNextInputType()
    {
        var next = (int)currentInputType + 1;
        if (next > (int)ChatType.Whisper) next = 0;
        SetChatInputType((ChatType)next);
    }

    /// <summary>
    /// 현재 시간 포함한 채팅 텍스트 구성
    /// </summary>
    private string ComposeChatText()
    {
        string time = DateTime.Now.ToString("HH:mm");
        return $"[{time}] [{currentInputType}] [{ID}] : {inputField.text}";
    }

    /// <summary>
    /// 현재 뷰 타입(필터링)에 따라 메시지 표시 여부 설정
    /// </summary>
    public void SetCurrentViewType(int newType)
    {
        currentViewType = (ChatType)newType;

        foreach (var cell in chatList)
        {
            cell.gameObject.SetActive(currentViewType == ChatType.All || cell.ChatType == currentViewType);
        }
    }

    /// <summary>
    /// 채팅창 접기/펼치기 토글
    /// </summary>
    public void ToggleCollapseChatWindow()
    {
        isToggleCollapse = !isToggleCollapse;
        bool isExpanded = rectTransform.sizeDelta.y < currentSize.y;
        rectTransform.sizeDelta = isExpanded ? currentSize : new Vector2(originalSize.x, 200);
    }

    // === 채팅창 드래그 리사이즈 관련 ===

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isToggleCollapse) return;
        originalMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isToggleCollapse) return;

        Vector2 delta = eventData.position - originalMousePosition;
        Vector2 newSize = new(
            Mathf.Clamp(currentSize.x + delta.x, MinWidth, MaxWidth),
            Mathf.Clamp(currentSize.y + delta.y, MinHeight, MaxHeight));

        rectTransform.sizeDelta = newSize;
        AdjustFontSize(newSize);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isToggleCollapse) return;
        currentSize = rectTransform.sizeDelta;
    }

    /// <summary>
    /// 채팅 셀의 폰트 크기 및 너비 조정
    /// </summary>
    private void AdjustFontSize(Vector2 newSize)
    {
        foreach (var cell in chatList)
        {
            var text = cell.GetComponent<TextMeshProUGUI>();
            text.fontSize = CalculateChatFontSize();
            text.rectTransform.sizeDelta = new Vector2(newSize.x - 20, text.rectTransform.sizeDelta.y);
        }
    }

    /// <summary>
    /// 채팅창 사이즈에 따라 폰트 크기 계산
    /// </summary>
    private float CalculateChatFontSize()
    {
        float widthRatio = rectTransform.sizeDelta.x / originalSize.x;
        float heightRatio = rectTransform.sizeDelta.y / originalSize.y;
        return Mathf.Clamp(Mathf.Max(widthRatio, heightRatio) * MinFontSize, MinFontSize, MaxFontSize);
    }
}

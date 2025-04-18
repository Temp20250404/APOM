using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.EventSystems.PointerEventData;

/// <summary>
/// 바인딩할 UI 오브젝트 이름 정의 (이름은 Hierarchy 오브젝트 이름과 1:1 매칭돼야 함)
/// </summary>
public enum ChatObjects
{
    ParentContent,     // 채팅 메시지를 출력할 ScrollView의 Content 영역
    InputField,        // 채팅 입력 필드 (TMP_InputField)
    ChatTypeText,      // 채팅 타입 (Normal, Party 등) 표시 텍스트
    TextInput,          // 채팅 입력 텍스트 색상용 텍스트 오브젝트
    RectTransform,    // 채팅창 RectTransform
    ToggleCollapse, // 채팅창 접기/펼치기 버튼
    InputButton, // 채팅 입력 버튼
    AllViewToggle, // 전체 보기 전용 토글
    NormalViewToggle, // 일반 채팅 보기 전용 토글
    PartyViewToggle, // 파티 채팅 보기 전용 토글
    WhisperViewToggle, // 귓속말 보기 전용 토글
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

public class UI_Chat : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    // 프리팹: 채팅 메시지를 하나씩 출력할 오브젝트 (SubItem/ChatCell)
    private GameObject ChatCell;

    // UI 요소들
    private Transform parentContent; // 채팅 메시지를 출력할 ScrollView의 Content 영역
    private TMP_InputField inputField; // 채팅 입력 필드
    private TextMeshProUGUI chatTypeText; // 채팅 타입 (Normal, Party 등) 표시 텍스트
    private TextMeshProUGUI textInput;  // 채팅 입력 필드의 텍스트
    private Toggle toggleCollapse; // 채팅창 접기/펼치기 버튼
    private Button inputButton; // 채팅 입력 버튼
    private Toggle allViewToggle; // 전체 보기 전용 토글
    private Toggle normalViewToggle; // 일반 채팅 보기 전용 토글
    private Toggle partyViewToggle; // 파티 채팅 보기 전용 토글
    private Toggle whisperViewToggle; // 귓속말 보기 전용 토글

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
    private const float MinWidth = 400;
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
        rectTransform = GetObject((int)ChatObjects.RectTransform)?.GetComponent<RectTransform>();
        toggleCollapse = GetObject((int)ChatObjects.ToggleCollapse)?.GetComponent<Toggle>();
        inputButton = GetObject((int)ChatObjects.InputButton)?.GetComponent<Button>();
        allViewToggle = GetObject((int)ChatObjects.AllViewToggle)?.GetComponent<Toggle>();
        normalViewToggle = GetObject((int)ChatObjects.NormalViewToggle)?.GetComponent<Toggle>();
        partyViewToggle = GetObject((int)ChatObjects.PartyViewToggle)?.GetComponent<Toggle>();
        whisperViewToggle = GetObject((int)ChatObjects.WhisperViewToggle)?.GetComponent<Toggle>();

        // UI 요소들에 이벤트 리스너 추가
        toggleCollapse.onValueChanged.AddListener(ToggleCollapseChatWindow);
        inputButton.onClick.AddListener(UpdateChat); // 버튼 클릭 시 채팅 전송
        inputField.onEndEdit.AddListener(OnEndEditEventMethod); // 입력 필드에서 엔터 입력 시 처리
        allViewToggle.onValueChanged.AddListener((isOn) => SetCurrentViewType((int)ChatType.All));
        normalViewToggle.onValueChanged.AddListener((isOn) => SetCurrentViewType((int)ChatType.Normal));
        partyViewToggle.onValueChanged.AddListener((isOn) => SetCurrentViewType((int)ChatType.Party));
        whisperViewToggle.onValueChanged.AddListener((isOn) => SetCurrentViewType((int)ChatType.Whisper));

        currentInputType = ChatType.Normal;
        SetChatInputType(currentInputType); // 기본 채팅 타입 설정

        originalSize = rectTransform.sizeDelta;
        currentSize = originalSize;
    }

    private bool justClosed = false;

    /// <summary>
    /// 키 입력 핸들링 (엔터, 탭)
    /// </summary>
    private void Update()
    {
        if (justClosed)
        {
            justClosed = false;
            return; // 한 프레임 동안 Enter 무시

        }
        if (Input.GetKeyDown(KeyCode.Return) && !inputField.isFocused)
        {

            rectTransform.gameObject.SetActive(true);
            inputField.ActivateInputField(); // 포커스 없을 때 엔터 → 입력창 활성화
            Debug.Log("InputField activated");
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused)
            SetNextInputType(); // 포커스 있을 때 탭 → 입력 채팅 타입 순환

    }

    /// <summary>
    /// 엔터 입력 후 채팅 전송 처리
    /// </summary>
    public void OnEndEditEventMethod(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return) && text == string.Empty)
        {
            inputField.DeactivateInputField(); // 먼저 비활성화
            rectTransform.gameObject.SetActive(false); // 채팅 타입 텍스트 활성화
            StartCoroutine(EnterDelay());
        }
        else if (Input.GetKeyDown(KeyCode.Return))
            UpdateChat();

        if (Input.GetKeyDown(KeyCode.Escape))
            inputField.DeactivateInputField(); // 입력창 비활성화
    }
    
    IEnumerator EnterDelay()
    {
        justClosed = true;
        yield return null; // 1프레임 쉬기
    }

    /// <summary>
    /// 채팅 텍스트를 인스턴스화하여 출력
    /// </summary>
    public void UpdateChat()
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return;

        string strChatText = ComposeChatText();

        // 서버에 채팅 메시지 전송
        CS_CHAT ptk = new CS_CHAT();
        ptk.Message = strChatText;
        ptk.Channel = (uint)currentInputType;
        // ptk.ChatType = (int)currentInputType; // 채팅 타입을 int로 변환하여 전송
        Managers.Network.Send(ptk);

        // 필드 초기화
        inputField.text = string.Empty;
    }


    public void ServerByChat(string message, uint channel)//int chatType
    {
        ChatType type = (ChatType)channel;
        Color color = ChatColors[type];

        var chatCell = Instantiate(ChatCell, parentContent).GetComponent<ChatCell>();
        chatCell.Setup(type, color, message);
        //chatCell.GetComponent<TextMeshProUGUI>().fontSize = CalculateChatFontSize();

        chatList.Add(chatCell);
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
    public void ToggleCollapseChatWindow(bool isOn)
    {
        isToggleCollapse = !isToggleCollapse;
        rectTransform.sizeDelta = isOn ? currentSize : new Vector2(originalSize.x, 200);
    }


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
        //AdjustFontSize(newSize);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isToggleCollapse) return;
        currentSize = rectTransform.sizeDelta;
    }

    /// <summary>
    /// 채팅 셀의 폰트 크기 및 너비 조정
    /// </summary>
    //private void AdjustFontSize(Vector2 newSize)
    //{
    //    foreach (var cell in chatList)
    //    {
    //        var text = cell.GetComponent<TextMeshProUGUI>();
    //        text.fontSize = CalculateChatFontSize();
    //        text.rectTransform.sizeDelta = new Vector2(newSize.x - 20, text.rectTransform.sizeDelta.y);
    //    }
    //}

    /// <summary>
    /// 채팅창 사이즈에 따라 폰트 크기 계산
    /// </summary>
    //private float CalculateChatFontSize()
    //{
    //    float widthRatio = rectTransform.sizeDelta.x / originalSize.x;
    //    float heightRatio = rectTransform.sizeDelta.y / originalSize.y;
    //    return Mathf.Clamp(Mathf.Max(widthRatio, heightRatio) * MinFontSize, MinFontSize, MaxFontSize);
    //}
}

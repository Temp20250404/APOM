using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public enum ChatType
{
    Normal = 0,   // 일반 대화
    Party,        // 파티 대화
    Whisper,      // 귓속말 대화
    All,          // 모든 대화 (보기용)
    Count         // ChatType 항목의 수
}

public class ChatController : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField]
    private GameObject textChatPrefab; // 대화 텍스트 프리팹 (UI에서 생성될 텍스트 오브젝트)
    [SerializeField]
    private Transform parentContent; // 대화가 출력될 ScrollView의 Content 영역 (스크롤 가능 리스트)
    [SerializeField]
    private TMP_InputField inputField; // 대화 입력창 (유저가 텍스트를 입력하는 곳)

    [SerializeField]
    private TextMeshProUGUI chatTypeText; // 현재 대화 속성 변경 버튼에 표시될 텍스트 (예: Normal, Party, Whisper)
    [SerializeField]
    private TextMeshProUGUI textInput; // 대화 입력창의 텍스트 색상

    private ChatType currentInputType; // 현재 대화 입력 속성 (예: Normal, Party, Whisper)
    private Color currentTextColor; // 현재 대화 속성에 따른 텍스트 색상

    private List<ChatCell> chatList; // 대화창에 출력된 모든 대화를 보관하는 리스트
    private ChatType currentViewType; // 현재 선택된 대화 보기 속성

    private string ID = "Player"; // 대화에서 사용할 ID (예: 유저의 이름)

    private RectTransform rectTransform; // RectTransform 컴포넌트 (UI 요소의 위치와 크기를 조정하는 데 사용)
    private Vector2 originalSize; // 원래 크기 저장 (UI 요소의 크기를 조정하기 위해 사용)
    private Vector2 originalMousePosition; // 원래 마우스 위치 저장 (드래그 시작 시점)
    private Vector2 currentSize; // 현재 크기 저장 (UI 요소의 크기를 조정하기 위해 사용)

    private float minChatSize = 20f; // 최소 너비 (채팅 텍스트의 최소 크기)
    private float maxChatSize = 40f; // 최대 너비 (채팅 텍스트의 최대 크기)

    private bool isToggleCollapse; // 대화창 접기/펼치기 상태 (true: 접힘, false: 펼쳐짐)

    private void Awake()
    {
        chatList = new List<ChatCell>(); // 대화 셀들을 저장할 리스트 초기화

        currentInputType = ChatType.Normal; // 기본 대화 유형 설정 (Normal)
        currentTextColor = Color.red; // 기본 텍스트 색상 설정 (red)
        textInput.color = Color.red; // 대화 입력창의 텍스트 색상 설정
    }

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>(); // RectTransform 컴포넌트 가져오기
        originalSize = rectTransform.sizeDelta; // 원래 크기 저장
        currentSize = originalSize; // 현재 크기 초기화
        Debug.Log(originalSize);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && inputField.isFocused == false)
        {
            inputField.ActivateInputField(); // 대화 입력창 활성화
        }

        if (Input.GetKeyDown(KeyCode.Tab) && inputField.isFocused == true)
        {
            SetCurrentInputType(); // Tab 키 눌렀을 때 대화 입력 타입 전환
        }
    }

    // EndEditEvent가 호출될 때 대화 텍스트 입력 처리
    public void OnEndEditEventMethod()
    {
        if (Input.GetKeyDown(KeyCode.Return)) // Enter키가 눌리면 대화 처리
        {
            UpdateChat();
        }
    }

    // 대화 업데이트 메서드
    public void UpdateChat()
    {
        if (inputField.text.Equals("")) return; // 입력된 텍스트가 없으면 처리하지 않음

        GameObject clone = Instantiate(textChatPrefab, parentContent); // 텍스트 프리팹을 부모 Content에 인스턴스화
        ChatCell chatCell = clone.GetComponent<ChatCell>(); // ChatCell 컴포넌트를 가져옴

        chatCell.Setup(currentInputType, currentTextColor, SetupText()); // 셀에 대화 정보 설정
        chatCell.GetComponent<TextMeshProUGUI>().fontSize = NewChatSize(); // 폰트 크기 설정

        inputField.text = ""; // 대화 입력창 초기화

        chatList.Add(chatCell); // 새로 생성된 대화 셀을 리스트에 추가
    }

    // ChatType에 맞는 텍스트 색상을 반환하는 메서드
    private Color ChatTypeToColor(ChatType type)
    {
        Color[] colors = new Color[] { Color.red, Color.blue, Color.magenta }; // 각 ChatType에 대응하는 색상 배열
        return colors[(int)type]; // 배열 인덱스를 통해 색상 반환
    }

    // ChatType에 맞는 텍스트 이름을 반환하는 메서드
    private string ChatTypeToName(ChatType type)
    {
        string[] names = new string[] { "Normal", "Party", "Whisper" }; // 각 ChatType에 대응하는 이름 배열
        return names[(int)type]; // 배열 인덱스를 통해 이름 반환
    }

    // 대화 텍스트에 시간을 추가하고 설정하는 메서드
    private string SetupText()
    {
        string time = DateTime.Now.ToString("HH:mm"); // 현재 시간 가져오기 (HH:mm 형식)
        string text = $"[{time}] [{currentInputType}] [{ID}] : {inputField.text}"; // 최종 텍스트 구성

        return text;
    }

    // 대화 입력 타입 변경 메서드
    public void SetCurrentInputType()
    {
        // 입력 타입을 순차적으로 변경 (Normal -> Party -> Whisper -> Normal...)
        currentInputType = (int)currentInputType < (int)ChatType.Count - 2 ? currentInputType + 1 : 0;
        chatTypeText.text = ChatTypeToName(currentInputType); // 버튼 텍스트에 현재 타입 이름 표시
        currentTextColor = ChatTypeToColor(currentInputType); // 현재 타입에 맞는 텍스트 색상 설정
        textInput.color = currentTextColor == Color.white ? Color.black : currentTextColor; // 입력창 텍스트 색상 변경
    }

    // 대화 보기 타입 변경 메서드
    public void SetCurrentViewType(int newType)
    {
        currentViewType = (ChatType)newType;

        if (currentViewType == ChatType.All) // 'All'로 설정된 경우 모든 대화 셀을 활성화
        {
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(true);
            }
        }
        else // 그 외 타입은 해당 타입에 맞는 대화만 활성화
        {
            for (int i = 0; i < chatList.Count; ++i)
            {
                chatList[i].gameObject.SetActive(chatList[i].ChatType == currentViewType);
            }
        }
    }

    public void ToggleCollapseChatWindow() // 대화창 접기/펼치기 메서드
    {
        isToggleCollapse = !isToggleCollapse; // 현재 상태 반전
        bool isOn = rectTransform.sizeDelta.y < currentSize.y; // 현재 상태 확인 (펼쳐져 있는지 접혀 있는지)
        rectTransform.sizeDelta = isOn ? currentSize : new Vector2(originalSize.x, 200); // 원래 크기로 복원
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(isToggleCollapse) return; // 대화창이 접혀 있는 경우 드래그 종료 처리 안 함
        currentSize = rectTransform.sizeDelta; // 드래그 종료 시 원래 크기 저장
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isToggleCollapse) return; // 대화창이 접혀 있는 경우 드래그 처리 안 함
        Vector2 delta = eventData.position - originalMousePosition; // 드래그한 거리 계산

        rectTransform.sizeDelta = new Vector2(Mathf.Clamp(currentSize.x + delta.x, 500, 1600),
            Mathf.Clamp(currentSize.y + delta.y, 200, 800)); // 크기 조정

        AdjustFontSize(delta);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isToggleCollapse) return; // 대화창이 접혀 있는 경우 드래그 시작 처리 안 함
        originalMousePosition = eventData.position; // 드래그 시작 시점 저장
    }

    private void AdjustFontSize(Vector2 delta)
    {
        foreach (ChatCell chatCell in chatList)
        {
            var cell = chatCell.GetComponent<TextMeshProUGUI>();
            cell.fontSize = NewChatSize(); // 각 대화 셀의 폰트 크기 설정
            cell.rectTransform.sizeDelta = new Vector2(Mathf.Clamp(currentSize.x + delta.x -20, 480, 1600), cell.rectTransform.sizeDelta.y); // 셀 크기 조정
        }   
    }

    private float NewChatSize()
    {
        // 현재 크기에 따라 폰트 크기 조정
        float widthRatio = rectTransform.sizeDelta.x / originalSize.x;
        float heightRatio = rectTransform.sizeDelta.y / originalSize.y;
        float newChatSize = Mathf.Clamp(Mathf.Max(widthRatio * minChatSize, heightRatio * minChatSize), minChatSize, maxChatSize);

        return newChatSize; // 조정된 폰트 크기 반환
    }
}

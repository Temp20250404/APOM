using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum ChatType
{
    Normal = 0,   // 일반 대화
    Party,        // 파티 대화
    Whisper,      // 귓속말 대화
    All,          // 모든 대화 (보기용)
    Count         // ChatType 항목의 수
}

public class ChatController : MonoBehaviour
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

    private void Awake()
    {
        chatList = new List<ChatCell>(); // 대화 셀들을 저장할 리스트 초기화

        currentInputType = ChatType.Normal; // 기본 대화 유형 설정 (Normal)
        currentTextColor = Color.red; // 기본 텍스트 색상 설정 (red)
        textInput.color = Color.red; // 대화 입력창의 텍스트 색상 설정
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
}

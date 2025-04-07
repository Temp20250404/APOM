using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatCell : MonoBehaviour
{
    public ChatType ChatType { private set; get; } // 해당 대화 셀의 ChatType을 저장 (읽기 전용)

    // ChatCell을 설정하는 메서드 (ChatType, 색상, 텍스트 데이터를 설정)
    public void Setup(ChatType type, Color color, string textData)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI 컴포넌트 가져오기

        ChatType = type; // 해당 셀의 ChatType 설정
        text.color = color; // 텍스트 색상 설정
        text.text = textData; // 텍스트 내용 설정
    }
}

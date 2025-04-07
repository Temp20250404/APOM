using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ChatCell : MonoBehaviour
{
    public ChatType ChatType { private set; get; } // �ش� ��ȭ ���� ChatType�� ���� (�б� ����)

    [SerializeField] private TextMeshProUGUI chatText;

    // ChatCell�� �����ϴ� �޼��� (ChatType, ����, �ؽ�Ʈ �����͸� ����)
    public void Setup(ChatType type, Color color, string textData)
    {
        ChatType = type; // �ش� ���� ChatType ����
        chatText.color = color; // �ؽ�Ʈ ���� ����
        chatText.text = textData; // �ؽ�Ʈ ���� ����
    }
}

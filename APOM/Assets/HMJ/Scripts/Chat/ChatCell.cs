using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatCell : MonoBehaviour
{
    public ChatType ChatType { private set; get; } // �ش� ��ȭ ���� ChatType�� ���� (�б� ����)

    // ChatCell�� �����ϴ� �޼��� (ChatType, ����, �ؽ�Ʈ �����͸� ����)
    public void Setup(ChatType type, Color color, string textData)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>(); // TextMeshProUGUI ������Ʈ ��������

        ChatType = type; // �ش� ���� ChatType ����
        text.color = color; // �ؽ�Ʈ ���� ����
        text.text = textData; // �ؽ�Ʈ ���� ����
    }
}

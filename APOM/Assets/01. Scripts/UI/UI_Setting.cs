using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    public Slider sliderMaster; // ��ü����
    public InputField inputMaster; // ��ü���� ����Ű ����

    public Slider sliderBGM; // �����
    public Slider sliderSFX; // ȿ����
    public Slider sliderCombat; // ĳ���ͽ�ų, ���� Ÿ����

    public Toggle muteToggle;
    public Button VideoButton;
    public Button AudioButton;
    public Button applyButton;
    public Button resetButton;
    public Button confirmButton;
    public Button closeButton;

    private float defaultVolum = 1f;

    private void Start()
    {
        sliderMaster.value = 1f;
        inputMaster.text = "100%";

        sliderBGM.value = 0.6f;
        sliderSFX.value = 0.6f;
        sliderCombat.value = 0.6f;

        muteToggle.isOn = false;

        sliderMaster.onValueChanged.AddListener(OnMasterSliderChanged);
        inputMaster.onEndEdit.AddListener(OnMasterInputChanged);
        muteToggle.onValueChanged.AddListener(OnMuteToggled);

        AudioButton.onClick.AddListener(OnAudioClicked);
        applyButton.onClick.AddListener(OnApplyClicked);
        resetButton.onClick.AddListener(OnResetClicked);
        confirmButton.onClick.AddListener(OnConfirmClicked);
        closeButton.onClick.AddListener(OnCloseClicked);
    }

    void OnMasterSliderChanged(float value)
    {
        inputMaster.text = (value*100).ToString("F0");
        if (!muteToggle.isOn)
            AudioListener.volume = value;
    }

    void OnMasterInputChanged(string input)
    {
        if (float.TryParse(input, out float value))
        {
            value = Mathf.Clamp(value, 0, 100);
            sliderMaster.value = value / 100f;
            if (!muteToggle.isOn)
                AudioListener.volume = sliderMaster.value;
        }
    }

    void OnMuteToggled(bool isMuted)
    {
        AudioListener.volume = isMuted ? 0 : sliderMaster.value;
    }

    void OnAudioClicked()
    {

    }
    
    void OnApplyClicked()
    {
        Debug.Log("���� �����");
    }

    void OnResetClicked()
    {
        sliderMaster.value = 1;
        inputMaster.text = "100%";
        muteToggle.isOn = false;

        sliderBGM.value = 0.6f;
        sliderSFX.value = 0.6f;
        sliderCombat.value = 0.6f;
    }
    
    void OnConfirmClicked()
    {
        Debug.Log("���� ����Ϸ�");
        // ���� ���� �ڸ�
    }

    void OnCloseClicked()
    {
        gameObject.SetActive(false);
    }
}

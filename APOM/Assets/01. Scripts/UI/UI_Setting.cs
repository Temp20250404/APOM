using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    [Header("�����̴�")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    [Header("�ؽ�Ʈ")]
    public TMP_Text masterPercentText;
    public TMP_Text bgmPercentText;
    public TMP_Text sfxPercentText;
    public TMP_Text uiPercentText;

    [Header("���Ұ�")]
    public Toggle muteToggle;

    [Header("��ư")]
    public Button applyButton;
    public Button confirmButton;
    public Button resetButton;

    private float defaultMasterVoulume = 1f;
    private float defaultOtherVolume = 0.6f;

    private void Start()
    {
        LoadSettings();

        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        bgmSlider.onValueChanged.AddListener(value => UpdateSliderText(bgmPercentText, value));
        sfxSlider.onValueChanged.AddListener(value => UpdateSliderText(sfxPercentText, value));
        uiSlider.onValueChanged.AddListener(value => UpdateSliderText(uiPercentText, value));

        muteToggle.onValueChanged.AddListener(ToggleMute);

        applyButton.onClick.AddListener(ApplySettings);
        confirmButton.onClick.AddListener(() => {
            ApplySettings();
            gameObject.SetActive(false);
        });

        resetButton.onClick.AddListener(ResetCurrentTab);
    }

    private void UpdateMasterVolumee(float value)
    {
        UpdateSliderText
    }
}    
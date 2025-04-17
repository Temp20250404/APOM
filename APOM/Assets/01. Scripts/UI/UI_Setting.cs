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

    [Header("�⺻Ű")]
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode equipmentKey = KeyCode.P;
    public int skill1MouseButton = 0; // ��Ŭ��
    public int skill2MouseButton = 1; // ��Ŭ��

    [Header("UI ������&�θ�")]
    public GameObject inventoryPrefab;
    public GameObject equipmentPrefab;
    public Transform canvasTransform;

    private GameObject inventoryInstance;
    private GameObject equipmentInstance;

    [Header("Ű ���ε� UI")]
    public Button inventoryKeyButton;
    public TMP_Text inventoryKeyText;

    public Button equipmentKeyButton;
    public TMP_Text equipmentKeyText;

    private string waitingForKey = null;

    [Header("�⺻����")]
    private float defaultMasterVolume = 1f;
    private float defaultOtherVolume = 0.6f;
    
    private void Start()
    {
        LoadSettings();
        //RefreshKeyTexts();

        inventoryKeyButton.onClick.AddListener(() =>
        {
            waitingForKey = "Inventory";
            inventoryKeyText.text = "�Է� ���...";
        });

        equipmentKeyButton.onClick.AddListener(() =>
        {
            waitingForKey = "Equipment";
            equipmentKeyText.text = "�Է� ���...";
        });

        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        masterSlider.onValueChanged.AddListener(UpdateMasterVolume);
        bgmSlider.onValueChanged.AddListener(value => UpdateSliderText(bgmPercentText, value));
        sfxSlider.onValueChanged.AddListener(value => UpdateSliderText(sfxPercentText, value));
        uiSlider.onValueChanged.AddListener(value => UpdateSliderText(uiPercentText, value));

        //muteToggle.onValueChanged.AddListener(ToggleMute);

        applyButton.onClick.AddListener(ApplySettings);
        confirmButton.onClick.AddListener(() => 
        {
            ApplySettings();
            gameObject.SetActive(false);
        });

        resetButton.onClick.AddListener(ResetCurrentTap);
    }

    private void Update()
    {
        if (Input.GetKeyDown(inventoryKey)) // �κ��丮 ����
        {
            ToggleUI(ref inventoryInstance, inventoryPrefab);
        }

        if (Input.GetKeyDown(equipmentKey))
        {
            ToggleUI(ref equipmentInstance, equipmentPrefab);
        }

        if (Input.GetMouseButtonDown(skill1MouseButton))
        {
            Debug.Log("��ų 1 �ߵ� (��Ŭ��)");
            // ��ų1 �ߵ� ���� �ڸ�
        }

        if (Input.GetMouseButtonDown(skill2MouseButton))
        {
            Debug.Log("��ų 2 �ߵ� (��Ŭ��)");
            // ��ų 2 �ߵ� ���� �ڸ�
        }
    }

    private void UpdateMasterVolume(float value)
    {
        UpdateSliderText(masterPercentText, value);
        muteToggle.isOn = Mathf.Approximately(value, 0f);
    }

    private void UpdateSliderText(TMP_Text text, float value)
    {
        text.text = Mathf.RoundToInt(value*100f) + "%";
    }

    private void ApplySettings()
    {
        PlayerPrefs.SetFloat("MasterVolume", masterSlider.value);
        PlayerPrefs.SetFloat("BGMVlume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.SetFloat("UIVolume", uiSlider.value);
        PlayerPrefs.SetFloat("UIVolume", uiSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        float master = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
        float bgm = PlayerPrefs.GetFloat("BGMVolume", defaultOtherVolume);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", defaultOtherVolume);
        float ui = PlayerPrefs.GetFloat("UIVolume", defaultOtherVolume);

        masterSlider.value = master;
        bgmSlider.value = bgm;
        sfxSlider.value = sfx;
        uiSlider.value = ui;

        UpdateMasterVolume(master);
        UpdateSliderText(bgmPercentText, bgm);
        UpdateSliderText(sfxPercentText, sfx);
        UpdateSliderText(uiPercentText, ui);
    }

    private void ResetCurrentTap()
    {
        masterSlider.value = defaultMasterVolume;
        bgmSlider.value = defaultOtherVolume;
        sfxSlider.value = defaultOtherVolume;
        uiSlider.value = defaultOtherVolume;
    }

    void ToggleUI(ref GameObject instance, GameObject Prefab)
    {
        if (instance == null)
        {
            instance = Instantiate(Prefab, canvasTransform);
        }
        else
        {
            instance.SetActive(!instance.activeSelf);
        }
    }

    private void ToggleMute()
    {

    }
}    
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Setting : MonoBehaviour
{
    [Header("슬라이더")]
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    [Header("텍스트")]
    public TMP_Text masterPercentText;
    public TMP_Text bgmPercentText;
    public TMP_Text sfxPercentText;
    public TMP_Text uiPercentText;

    [Header("음소거")]
    public Toggle muteToggle;

    [Header("버튼")]
    public Button applyButton;
    public Button confirmButton;
    public Button resetButton;

    [Header("기본키")]
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode equipmentKey = KeyCode.P;
    public int skill1MouseButton = 0; // 좌클릭
    public int skill2MouseButton = 1; // 우클릭

    [Header("UI 프리펩&부모")]
    public GameObject inventoryPrefab;
    public GameObject equipmentPrefab;
    public Transform canvasTransform;

    private GameObject inventoryInstance;
    private GameObject equipmentInstance;

    [Header("키 바인딩 UI")]
    public Button inventoryKeyButton;
    public TMP_Text inventoryKeyText;

    public Button equipmentKeyButton;
    public TMP_Text equipmentKeyText;

    private string waitingForKey = null;

    [Header("기본음량")]
    private float defaultMasterVolume = 1f;
    private float defaultOtherVolume = 0.6f;
    
    private void Start()
    {
        LoadSettings();
        //RefreshKeyTexts();

        inventoryKeyButton.onClick.AddListener(() =>
        {
            waitingForKey = "Inventory";
            inventoryKeyText.text = "입력 대기...";
        });

        equipmentKeyButton.onClick.AddListener(() =>
        {
            waitingForKey = "Equipment";
            equipmentKeyText.text = "입력 대기...";
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
        if (Input.GetKeyDown(inventoryKey)) // 인벤토리 열기
        {
            ToggleUI(ref inventoryInstance, inventoryPrefab);
        }

        if (Input.GetKeyDown(equipmentKey))
        {
            ToggleUI(ref equipmentInstance, equipmentPrefab);
        }

        if (Input.GetMouseButtonDown(skill1MouseButton))
        {
            Debug.Log("스킬 1 발동 (좌클릭)");
            // 스킬1 발동 로직 자리
        }

        if (Input.GetMouseButtonDown(skill2MouseButton))
        {
            Debug.Log("스킬 2 발동 (우클릭)");
            // 스킬 2 발동 로직 자리
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
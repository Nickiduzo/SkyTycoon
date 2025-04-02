using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour, IDataPersistence
{
    public static event Action<float> OnChangeScroll;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;

    [SerializeField] private GameObject optionsPanel;

    [Header("Dynamic")]
    [SerializeField] private GameObject gameOpenedPanel;
    [SerializeField] private TextMeshProUGUI moneyAmount;
    [SerializeField] private Button takeOfflineMoney;
    [SerializeField] private OfflineCalculator offlineCalculator;

    [SerializeField] private Button openSettingsButton;

    [Header("Settings")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Slider scrollSlider;
    [SerializeField] private Toggle screenMode;
    [SerializeField] private TextMeshProUGUI screenModeTitle;
    [SerializeField] private Button closeSettingsButton;
    private float currentVolume;

    private void Awake()
    {
        quitButton.onClick.AddListener(Exit);
        optionsButton.onClick.AddListener(OpenOptions);
        takeOfflineMoney.onClick.AddListener(TakeOfflineMoney);

        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);

        volumeSlider.onValueChanged.AddListener(OnSliderValueChanged);
        scrollSlider.onValueChanged.AddListener(OnSensetivityChanged);

        screenMode.onValueChanged.AddListener(OnToggleScreenChanged);
    }

    private void Start()
    {
        InitializeOfflinePanel();
    }

    #region Options
    private void OpenOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    private void InitializeOfflinePanel()
    {
        if (offlineCalculator.GetOfflineMoney() == 0) return;
        moneyAmount.text = offlineCalculator.GetOfflineMoney().ToString() + "$";
        gameOpenedPanel.SetActive(true);
    }

    private void TakeOfflineMoney()
    {
        offlineCalculator.CollectOfflineEarnings();
        gameOpenedPanel.SetActive(false);
    }
    #endregion

    #region Settings
    private void OpenSettings()
    {
        optionsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    private void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    private void OnSliderValueChanged(float value)
    {
        volumeSlider.value = value;
        currentVolume = value;
        AudioManager.Instance.SetVolume(currentVolume);
    }

    private void OnSensetivityChanged(float value)
    {
        scrollSlider.value = value;
        OnChangeScroll?.Invoke(value);
    }

    private void OnToggleScreenChanged(bool value)
    {
        Screen.fullScreen = value;
        screenModeTitle.text = value == true ? "Full Screen" : "Windowed";
    }

    #endregion

    private void OnDisable()
    {
        quitButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        takeOfflineMoney.onClick.RemoveAllListeners();

        closeSettingsButton.onClick.RemoveAllListeners();
        openSettingsButton.onClick.RemoveAllListeners();

        screenMode.onValueChanged.RemoveAllListeners();
    }

    private void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    public void LoadData(GameData data)
    {
        volumeSlider.value = data.audioSliderValue;
        scrollSlider.value = data.scrollSliderValue;
        Screen.fullScreen = data.screenMode;
        screenModeTitle.text = Screen.fullScreen == true ? "Full Screen" : "Windowed";
    }

    public void SaveData(ref GameData data)
    {
        data.audioSliderValue = volumeSlider.value;
        data.scrollSliderValue = scrollSlider.value;
        data.screenMode = Screen.fullScreen;
    }
}

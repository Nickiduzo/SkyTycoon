using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
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
    [SerializeField] private Slider sensetivitySlider;
    [SerializeField] private Toggle screenMode;
    [SerializeField] private TextMeshProUGUI screenModeTitle;
    [SerializeField] private Button closeSettingsButton;

    private void Awake()
    {
        quitButton.onClick.AddListener(Exit);
        optionsButton.onClick.AddListener(OpenOptions);
        takeOfflineMoney.onClick.AddListener(TakeOfflineMoney);

        openSettingsButton.onClick.AddListener(OpenSettings);
        closeSettingsButton.onClick.AddListener(CloseSettings);
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

    private void ChangeVolume()
    {
        // later
    }

    private void ChangeSensetivity()
    {
        // later
    }

    #endregion
    private void OnDisable()
    {
        quitButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        takeOfflineMoney.onClick.RemoveAllListeners();

        closeSettingsButton.onClick.RemoveAllListeners();
        openSettingsButton.onClick.RemoveAllListeners();
    }

    private void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

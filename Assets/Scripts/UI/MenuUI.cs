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

    private void Awake()
    {
        quitButton.onClick.AddListener(Exit);
        optionsButton.onClick.AddListener(OpenOptions);
        takeOfflineMoney.onClick.AddListener(TakeOfflineMoney);

        InitializeOfflinePanel();
    }


    private void OpenOptions()
    {
        optionsPanel.SetActive(!optionsPanel.activeSelf);
    }

    private void InitializeOfflinePanel()
    {
        if (offlineCalculator.GetOfflineMoney() == 0) return;
        gameOpenedPanel.SetActive(true);
        moneyAmount.text = offlineCalculator.GetOfflineMoney().ToString() + "$";
    }

    private void TakeOfflineMoney()
    {
        gameOpenedPanel.SetActive(false);
    }


    private void OnDisable()
    {
        quitButton.onClick.RemoveAllListeners();
        optionsButton.onClick.RemoveAllListeners();
        takeOfflineMoney.onClick.RemoveAllListeners();
    }

    private void Exit()
    {
        Application.Quit();

        #if UNITY_EDITOR 
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}

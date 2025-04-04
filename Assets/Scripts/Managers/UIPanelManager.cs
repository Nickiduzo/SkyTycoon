using UnityEngine;

public class UIPanelManager : MonoBehaviour
{
    public static UIPanelManager Instance;

    [SerializeField] private GameObject[] uiPanels;

    public bool panelIsActive = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public bool IsClosePanels()
    {
        if (panelIsActive) return false;
        foreach (var panel in uiPanels)
        {
            if (panel.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true; 
    }
}

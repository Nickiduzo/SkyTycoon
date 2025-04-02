using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HintManager : MonoBehaviour
{
    public static HintManager Instance;

    [SerializeField] private GameObject notEnoughtMoney;
    [SerializeField] private GameObject noMoreHallTown;

    [SerializeField] private GameObject noSuitableFactory;
    [SerializeField] private TextMeshProUGUI factoryName;
    [SerializeField] private Image factoryImage;
    [SerializeField] private Sprite[] factoriesSprites;

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

    public void MoneyAlert()
    {
        AudioManager.Instance.Play("Alert");
        notEnoughtMoney.SetActive(true);
        StartCoroutine(AlertCoroutine(notEnoughtMoney, 2.5f));
    }

    public void HallPlacedAlert()
    {
        AudioManager.Instance.Play("Alert");
        noMoreHallTown.SetActive(true);
        StartCoroutine(AlertCoroutine(noMoreHallTown, 2.5f));
    }

    public void NoSuitableFactoryAlert(Building building)
    {
        AudioManager.Instance.Play("Alert");
        switch (building.name)
        {
            case "AppliancesStore":
                factoryName.text = "Appliances Factory";
                factoryImage.sprite = GetSuitable("AppliancesFactory");
                break;
            case "CarStore":
                factoryName.text = "Car Factory";
                factoryImage.sprite = GetSuitable("CarFactory");
                break;
            case "ChemicalStore":
                factoryName.text = "Chemical Factory";
                factoryImage.sprite = GetSuitable("ChemicalFactory");
                break;
            case "ElectronicStore":
                factoryName.text = "Electronic Factory";
                factoryImage.sprite = GetSuitable("ElectronicFactory");
                break;
            case "SoftwareStore":
                factoryName.text = "Software Studio";
                factoryImage.sprite = GetSuitable("SoftwareCompany");
                break;
            case "VideoGamesStore":
                factoryName.text = "Video Studio";
                factoryImage.sprite = GetSuitable("StudioVideoGames");
                break;
            default: Debug.LogError("No factory for this store...");
                break;

        }
        noSuitableFactory.SetActive(true);
        StartCoroutine(AlertCoroutine(noSuitableFactory, 2.5f));
    }

    private Sprite GetSuitable(string name)
    {
        return factoriesSprites.FirstOrDefault(x => x.name == name);
    }

    private IEnumerator AlertCoroutine(GameObject prefab, float duration)
    {
        yield return new WaitForSeconds(duration);
        prefab.SetActive(false);
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hall : Building
{
    [SerializeField] private GameObject hallInformation;
    [SerializeField] private TextMeshProUGUI hallTyreTitle;
    [SerializeField] private TextMeshProUGUI hallIncreaseFactor;
    [SerializeField] private TextMeshProUGUI hallPrice;
    [SerializeField] private Button increaseTyre;
    [SerializeField] private Button closeTyre;

    [SerializeField] private HallTyres[] hallTyres;

    public int currentTyre = 0;

    private void Start()
    {
        closeTyre.onClick.AddListener(CloseHallInformation);
        increaseTyre.onClick.AddListener(IncreaseHallTyre);

        UpdateHallInformation();
    }

    private void CloseHallInformation()
    {
        hallInformation.SetActive(false);
    }

    private void IncreaseHallTyre()
    {            
        if (currentTyre < hallTyres.Length - 1 &&
            MoneyManager.Instance.moneyAmount >= hallTyres[currentTyre].price)
        {
            MoneyManager.Instance.DecreaseMoney(hallTyres[currentTyre].price);
            MoneyManager.Instance.IncreaseDiamonds(hallTyres[currentTyre].reward);

            currentTyre++;
            
            UpdateHallInformation();
        }
    }

    private void UpdateHallInformation()
    {
        hallTyreTitle.text = "Tyre - " + currentTyre.ToString();
        hallIncreaseFactor.text = "IF - " + hallTyres[currentTyre].increaseFactor.ToString();
        hallPrice.text = " - " + hallTyres[currentTyre].price.ToString() + "$";

        CheckMoney();
    }

    private void OnMouseDown()
    {
        if (isPlaced)
        {
            hallTyreTitle.text = "Tyre - " + currentTyre.ToString();
            hallIncreaseFactor.text = "IF - " + hallTyres[currentTyre].increaseFactor.ToString();
            hallPrice.text = " - " + FormatMoney(hallTyres[currentTyre].price) + "$";
            
            CheckMoney();
            
            hallInformation.SetActive(true);
        }
    }

    public int GetCurrentTyre()
    {
        return currentTyre;
    }

    public float GetPriceForNextTyre()
    {
        return hallTyres[currentTyre].price;
    }

    private void CheckMoney()
    {
        if (MoneyManager.Instance.moneyAmount >= hallTyres[currentTyre].price)
        {
            increaseTyre.interactable = true;
        }
        else
        {
            increaseTyre.interactable = false;
        }
    }

    private void OnDisable()
    {
        closeTyre.onClick.RemoveAllListeners();
        increaseTyre.onClick.RemoveAllListeners();
    }

    public float GetCurrentIncreaseFactor()
    {
        return hallTyres[currentTyre].increaseFactor;
    }

    private string FormatMoney(float amount)
    {
        if (amount >= 1_000_000_000) return (amount / 1_000_000_000f).ToString("0.##") + "B";
        if (amount >= 1_000_000) return (amount / 1_000_000f).ToString("0.##") + "M";
        if (amount >= 1_000) return (amount / 1_000f).ToString("0.##") + "K";

        return amount.ToString("N0");
    }
}

[System.Serializable]
public class HallTyres
{
    public int tyre;
    public float price;
    public float reward;
    public float increaseFactor;
}
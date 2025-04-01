using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hall : Building
{
    [SerializeField] private GameObject hallInformation;
    [SerializeField] private TextMeshProUGUI hallTyre;
    [SerializeField] private TextMeshProUGUI hallIncreaseFactor;
    [SerializeField] private TextMeshProUGUI hallPrice;
    [SerializeField] private Button increaseTyre;
    [SerializeField] private Button closeTyre;
    [SerializeField] private HallTyre[] hallTyres;

    private int currentTyre = 0;
    public override void SetNormal()
    {
        base.SetNormal();
        if(currentTyre == 0)
        {
            currentTyre = 1;
        }
    }

    private void Start()
    {
        closeTyre.onClick.AddListener(CloseHallInformation);
        increaseTyre.onClick.AddListener(IncreaseHallTyre);

        if (MoneyManager.Instance.moneyAmount < hallTyres[currentTyre].priceForTyre)
        {
            increaseTyre.interactable = false;
        }
        else
        {
            increaseTyre.interactable = true;
        }
    }

    private void CloseHallInformation()
    {
        hallInformation.SetActive(false);
    }

    private void IncreaseHallTyre()
    {
        if(currentTyre < hallTyres.Length - 1)
        {
            MoneyManager.Instance.DecreaseMoney(hallTyres[currentTyre].priceForTyre);
            MoneyManager.Instance.IncreaseDiamonds(hallTyres[currentTyre].diamondPresent);
            currentTyre++;
            UpdateHallInformation();
        }
    }

    private void UpdateHallInformation()
    {
        hallTyre.text = "Tyre - " + hallTyres[currentTyre].tyre.ToString();
        hallIncreaseFactor.text = "IF - " + hallTyres[currentTyre].increaseBuildingsFactor.ToString();
        hallPrice.text = " - " + hallTyres[currentTyre].priceForTyre.ToString() + "$";
    }

    private void OnMouseDown()
    {
        if (isPlaced)
        {
            hallTyre.text = "Tyre - " + hallTyres[currentTyre].tyre.ToString();
            hallIncreaseFactor.text = "IF - " + hallTyres[currentTyre].increaseBuildingsFactor.ToString();
            hallPrice.text = " - " + hallTyres[currentTyre].priceForTyre.ToString() + "$";
            hallInformation.SetActive(true);
        }
    }

    private void OnDisable()
    {
        closeTyre.onClick.RemoveAllListeners();
        increaseTyre.onClick.RemoveAllListeners();
    }
}

[System.Serializable]
public class HallTyre
{
    public int tyre;
    public float priceForTyre;
    public float diamondPresent;
    public float increaseBuildingsFactor; 
}

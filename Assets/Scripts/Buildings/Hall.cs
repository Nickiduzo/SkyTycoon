using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Hall : Building
{
    [SerializeField] private TextMeshProUGUI hallTyre;
    [SerializeField] private TextMeshProUGUI hallModifier;
    [SerializeField] private TextMeshProUGUI hallPrice;

    [SerializeField] private Button increaseButton;

    [SerializeField] private HallTyres[] hallTyres;

    public int currentTyre = 0;

    private void Start()
    {
        increaseButton.onClick.AddListener(IncreaseHallTyre);

        AddHoverSound(increaseButton);

        UpdateHallInformation();
    }

    private void IncreaseHallTyre()
    {            
        if (currentTyre < hallTyres.Length - 1 &&
            MoneyManager.Instance.moneyAmount >= hallTyres[currentTyre].price)
        {
            AudioManager.Instance.Play("Click");
            MoneyManager.Instance.DecreaseMoney(hallTyres[currentTyre].price);
            MoneyManager.Instance.IncreaseDiamonds(hallTyres[currentTyre].reward);

            currentTyre++;
            
            UpdateHallInformation();
        }
    }

    private void UpdateHallInformation()
    {
        hallTyre.text = "Tyre - " + currentTyre.ToString();
        hallModifier.text = "IF - " + hallTyres[currentTyre].increaseFactor.ToString();
        hallPrice.text = " - " + FormatMoney(hallTyres[currentTyre].price) + "$";

        CheckMoney();
    }

    private void CheckMoney()
    {
        if (MoneyManager.Instance.moneyAmount >= hallTyres[currentTyre].price)
        {
            increaseButton.interactable = true;
        }
        else
        {
            increaseButton.interactable = false;
        }
    }

    public float GetCurrentIncreaseFactor()
    {
        return hallTyres[currentTyre].increaseFactor;
    }

    public override void OpenBuildingUI()
    {
        hallTyre.text = "Tyre - " + currentTyre.ToString();
        hallModifier.text = "IF - " + hallTyres[currentTyre].increaseFactor.ToString();
        hallPrice.text = " - " + FormatMoney(hallTyres[currentTyre].price) + "$";
        base.OpenBuildingUI();
    }


    private string FormatMoney(float amount)
    {
        if (amount >= 1_000_000_000) return (amount / 1_000_000_000f).ToString("0.##") + "B";
        if (amount >= 1_000_000) return (amount / 1_000_000f).ToString("0.##") + "M";
        if (amount >= 1_000) return (amount / 1_000f).ToString("0.##") + "K";

        return amount.ToString("N0");
    }

    private void AddHoverSound(Button button)
    {
        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerEnter
        };

        entry.callback.AddListener((eventData) => PlayHoverSound());
        
        trigger.triggers.Add(entry);
    }

    private void PlayHoverSound()
    {
        AudioManager.Instance.Play("Select");
    }

    private void OnDisable()
    {
        increaseButton.onClick.RemoveAllListeners();
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
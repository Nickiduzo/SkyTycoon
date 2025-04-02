using TMPro;
using UnityEngine;

public class MoneyRepresentUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmount;
    [SerializeField] private TextMeshProUGUI diamondAmount;

    [SerializeField] private TextMeshProUGUI moneyPerHour;

    private void Update()
    {
        GetMoneyAmount();
        GetDiamondAmount();
        GetMoneyPerMinute();
    }

    public void BuyDiamonds(float amount)
    {
        MoneyManager.Instance.IncreaseDiamonds(amount);
    }

    public void BuyMoney(float amount)
    {
        MoneyManager.Instance.IncreaseMoney(amount);
    }

    private void GetMoneyPerMinute() => moneyPerHour.text = FormatMoney(MoneyManager.Instance.moneyPerMinute) + "$ / MIN";

    private void GetDiamondAmount() => diamondAmount.text = FormatMoney(MoneyManager.Instance.diamondsAmount);

    private void GetMoneyAmount() => moneyAmount.text = FormatMoney(MoneyManager.Instance.moneyAmount) + "$";

    private string FormatMoney(float amount)
    {
        if (amount >= 1_000_000_000) return (amount / 1_000_000_000f).ToString("0.##") + "B";
        if (amount >= 1_000_000) return (amount / 1_000_000f).ToString("0.##") + "M";
        if (amount >= 1_000) return (amount / 1_000f).ToString("0.##") + "K";

        return amount.ToString("N0");
    }
}

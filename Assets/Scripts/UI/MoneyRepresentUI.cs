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

    private void GetMoneyPerMinute() => moneyPerHour.text = MoneyManager.Instance.moneyPerMinute.ToString() + "$";

    private void GetDiamondAmount() => diamondAmount.text = MoneyManager.Instance.diamondsAmount.ToString();

    private void GetMoneyAmount() => moneyAmount.text = MoneyManager.Instance.moneyAmount.ToString() + "$";

}

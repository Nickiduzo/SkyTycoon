using TMPro;
using UnityEngine;

public class MoneyRepresentUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyAmount;

    [SerializeField] private TextMeshProUGUI moneyPerHour;

    private void Update()
    {
        GetMoneyAmount();

        GetMoneyPerMinute();
    }

    private void GetMoneyPerMinute() => moneyPerHour.text = MoneyManager.Instance.moneyPerMinute.ToString() + "$";

    private void GetMoneyAmount() => moneyAmount.text = MoneyManager.Instance.moneyAmount.ToString() + "$";

}

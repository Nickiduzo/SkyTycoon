using UnityEngine;

public class MoneyManager : MonoBehaviour, IDataPersistence
{
    public static MoneyManager Instance;

    public float moneyAmount;
    public float diamondsAmount;
    public float moneyPerMinute;

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

    public void DecreaseMoney(float amount)
    {
        if (amount > moneyAmount) return;
        moneyAmount -= amount;
    }
    
    public void IncreaseMoney(float amount)
    {
        moneyAmount += amount;
    }

    public void IncreaseMoneyPerMinute(float amount)
    {
        moneyPerMinute += amount;
    }

    public void IncreaseDiamonds(float amount)
    {
        diamondsAmount += amount;
    }

    public void DecreaseDiamonds(float amount)
    {
        if(amount > diamondsAmount) return;

        diamondsAmount -= amount;
    }

    public void LoadData(GameData data)
    {
        this.diamondsAmount = data.diamonds;
        this.moneyAmount = data.money;
        this.moneyPerMinute = data.moneyPerMinute;
    }

    public void SaveData(ref GameData data)
    {
        data.diamonds = this.diamondsAmount;
        data.money = this.moneyAmount;
        data.moneyPerMinute = this.moneyPerMinute;
    }
}

using System;
using UnityEngine;

public class OfflineCalculator : MonoBehaviour, IDataPersistence
{
    private float offlineIncome;

    public void LoadData(GameData data)
    {
        if (DateTime.TryParse(data.lastLogoutTime, out DateTime lastLogoutTime))
        {
            TimeSpan timePassed = DateTime.Now - lastLogoutTime;
            float minutesPassed = ((int)timePassed.TotalMinutes);

            offlineIncome = data.moneyPerMinute * minutesPassed;
        }
        else
        {
            offlineIncome = 0;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.lastLogoutTime = DateTime.Now.ToString();
    }

    public void CollectOfflineEarnings()
    {
        MoneyManager.Instance.IncreaseMoney(offlineIncome);
        offlineIncome = 0;
    }

    public float GetOfflineMoney()
    {
        return offlineIncome;
    }
}

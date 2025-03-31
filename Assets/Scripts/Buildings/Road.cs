using UnityEngine;

public class Road : Building
{
    public override void SetNormal()
    {
        base.SetNormal();
        MoneyManager.Instance.DecreaseMoney(BuildingData.Price);
    }
}

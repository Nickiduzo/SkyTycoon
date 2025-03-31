using UnityEngine;

public class School : Building
{
    public float increaseFactor;
    
    public override void SetNormal()
    {
        base.SetNormal();
        MoneyManager.Instance.DecreaseMoney(BuildingData.Price);
    }
}

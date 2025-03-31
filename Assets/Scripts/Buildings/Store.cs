using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : Building
{
    [SerializeField] private Image factorySlider;
    [SerializeField] private TextMeshProUGUI moneyPerHour;
    [SerializeField] private Transform buildingCanvas;

    private float timeToGetMoney = 3600f;
    private float coolDown;

    private Factory[] factories;
    private void Awake()
    {
        moneyPerHour.text = BuildingData.MoneyPerMin.ToString();
        coolDown = timeToGetMoney;
        factories = FindObjectsByType<Factory>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        LookAtBuildingCanvas();
        CountMoney();
    }

    private void CountMoney()
    {
        if (isPlaced)
        {
            UpdateBar(3600, coolDown);

            if(coolDown <= 0)
            {
                coolDown = timeToGetMoney;
                MoneyManager.Instance.IncreaseMoney(BuildingData.MoneyPerMin);
            }
            coolDown -= Time.deltaTime;
        }
    }

    public override void SetNormal()
    {
        if (factories.Length == 0) return;
        base.SetNormal();
        MoneyManager.Instance.IncreaseMoneyPerMinute(BuildingData.MoneyPerMin);
        MoneyManager.Instance.DecreaseMoney(BuildingData.Price);
    }

    private void UpdateBar(float max, float current)
    {
        factorySlider.fillAmount = current / max;
    }

    private void LookAtBuildingCanvas()
    {
        buildingCanvas.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
    }
}

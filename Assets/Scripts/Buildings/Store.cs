using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Store : Building
{
    [SerializeField] private Image factorySlider;
    [SerializeField] private TextMeshProUGUI moneyPerHour;
    [SerializeField] private Transform buildingCanvas;

    private float coolDown;

    private void Start()
    {
        moneyPerHour.text = BuildingData.MoneyPerMin.ToString();
        coolDown = BuildingData.TimeToEarn;
    }

    private void Update()
    {
        LookAtBuildingCanvas();
        CountMoney();
    }

    private void CountMoney()
    {
        if (!isPlaced) print(isPlaced);
        if (isPlaced)
        {
            UpdateBar(BuildingData.TimeToEarn, coolDown);

            if(coolDown <= 0)
            {
                coolDown = BuildingData.TimeToEarn;
                MoneyManager.Instance.IncreaseMoney(BuildingData.MoneyPerMin);
            }
            coolDown -= Time.deltaTime;
        }
    }

    private void UpdateBar(float max, float current)
    {
        factorySlider.fillAmount = current / max;
    }

    private void LookAtBuildingCanvas()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        cameraForward.y = 0;
        buildingCanvas.rotation = Quaternion.LookRotation(cameraForward);
    }
}

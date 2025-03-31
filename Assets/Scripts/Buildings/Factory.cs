using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Factory : Building
{
    [SerializeField] private Image factorySlider;
    [SerializeField] private TextMeshProUGUI moneyPerMinuteText;
    [SerializeField] private Transform buildingCanvas;

    private float timeToGetMoney = 5f;
    private float coolDown;

    private void Awake()
    {
        moneyPerMinuteText.text = BuildingData.MoneyPerMin.ToString();
        coolDown = timeToGetMoney;
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
            UpdateBar(100, coolDown);

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
        base.SetNormal();
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

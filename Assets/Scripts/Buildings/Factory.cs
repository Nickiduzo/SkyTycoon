using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Factory : Building, IDataPersistence
{
    [SerializeField] private Image factorySlider;
    [SerializeField] private TextMeshProUGUI moneyPerMinuteText;
    [SerializeField] private Transform buildingCanvas;

    private float coolDown;

    private void Start()
    {
        moneyPerMinuteText.text = BuildingData.MoneyPerMin.ToString();
        coolDown = BuildingData.MoneyPerMin;
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

    // AI
    public void LoadData(GameData data)
    {
        var savedBuilding = data.buildingsPlaced.FirstOrDefault(b => b.id == id);
        if (savedBuilding != null)
        {
            if (DateTime.TryParse(data.lastLogoutTime, out DateTime lastLogout))
            {
                float offlineTime = (float)(DateTime.Now - lastLogout).TotalSeconds;
                coolDown = Mathf.Max(0, savedBuilding.coolDown - offlineTime); // Вычитаем время оффлайна
            }
            else
            {
                coolDown = savedBuilding.coolDown;
                print("Can't detect time");
            }
        }
    }

    // close AI region

    public void SaveData(ref GameData data)
    {
        var savedBuilding = data.buildingsPlaced.FirstOrDefault(b => b.id == id);
        if (savedBuilding != null)
        {
            savedBuilding.coolDown = coolDown;
        }
    }
}

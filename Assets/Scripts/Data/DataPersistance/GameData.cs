using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float money;
    public float diamonds;
    public float moneyPerMinute;
    public string lastLogoutTime;
    public Vector3 cameraPosition;

    public List<BuildingDataSave> buildingsPlaced;
    public List<BuildingDataSave> borderRocksPlaced;
    public List<BuildingDataSave> rabishPlaced;

    public int hallTyre;
    public float hallIncreaseFactor;

    public float audioSliderValue;
    public float scrollSliderValue;
    public bool screenMode;

    public GameData()
    {
        this.money = 1000;
        this.diamonds = 0;
        this.moneyPerMinute = 0;
        this.lastLogoutTime = System.DateTime.Now.ToString();
        
        buildingsPlaced = new List<BuildingDataSave>();
        borderRocksPlaced = new List<BuildingDataSave>();
        rabishPlaced = new List<BuildingDataSave>();

        this.hallTyre = 0;
        this.hallIncreaseFactor = 0.95f;

        this.audioSliderValue = 0.8f;
        this.scrollSliderValue = 20f;

        cameraPosition = new Vector3(30, 0, 11.5f);

        this.screenMode = true;
    }
}

[System.Serializable]
public class BuildingDataSave
{
    public string id;
    public string prefabName;
    public Vector3 position;
    public int buildingRotation;
    public float coolDown;

    public BuildingDataSave(Building building)
    {
        this.id = building.id;
        this.prefabName = building.gameObject.name.Replace("(Clone)", "").Trim();
        this.position = building.position;
        this.buildingRotation = building.currentRotation;
    }
}


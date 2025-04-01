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
    public List<BuildingDataSave> rockPlaced;

    public GameData()
    {
        this.money = 0;
        this.diamonds = 0;
        this.moneyPerMinute = 0;
        buildingsPlaced = new List<BuildingDataSave>();
        rockPlaced = new List<BuildingDataSave>();
        this.lastLogoutTime = System.DateTime.Now.ToString();
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
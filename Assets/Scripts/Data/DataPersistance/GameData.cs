using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float money;
    public float diamonds;
    public float moneyPerMinute;

    public List<Building> buildings;

    public GameData()
    {
        this.money = 0;
        this.diamonds = 0;
        this.moneyPerMinute = 0;
        buildings = new List<Building>();
    }
}
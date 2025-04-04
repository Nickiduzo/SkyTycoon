using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private Material[] materials;

    [SerializeField] private string buildingName;

    [SerializeField] private float price = 0;
    [SerializeField] private float priceForRemove = 0;
    [SerializeField] private float moneyPerMinute = 0;
    [SerializeField] private float timeToEarn = 0;

    public float MoneyPerMin => moneyPerMinute;
    public float Price => price;

    public float RemovePrice => priceForRemove;

    public float TimeToEarn => timeToEarn;

    public string Name => buildingName;

    public Material[] GetMaterials()
    {
        return materials;
    }
}

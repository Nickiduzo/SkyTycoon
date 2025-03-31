using UnityEngine;

[CreateAssetMenu(fileName = "BuildingData", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    [SerializeField] private Material[] materials;

    [SerializeField] private float price = 0;
    [SerializeField] private float moneyPerMinute = 0;

    public float MoneyPerMin => moneyPerMinute;
    public float Price => price;
    public Material[] GetMaterials()
    {
        return materials;
    }
}

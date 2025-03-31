using System.Collections.Generic;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour, IDataPersistence
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public List<Building> buildingPrefabs;

    private Dictionary<string, Building> placedBuildings = new Dictionary<string, Building>();

    [Header("First")]
    [SerializeField][Range(0,1.0f)] private float rFirst;
    [SerializeField][Range(0,1.0f)] private float gFirst;
    [SerializeField][Range(0,1.0f)] private float bFirst;
    [SerializeField][Range(0,1.0f)] private float aFirst;

    [Header("Second")]
    [SerializeField][Range(0,1.0f)] private float rSecond;
    [SerializeField][Range(0,1.0f)] private float gSecond;
    [SerializeField][Range(0,1.0f)] private float bSecond;
    [SerializeField][Range(0,1.0f)] private float aSecond;

    private Camera mainCamera;
    private Building[,] grid;
    private Building flyingBuilding;


    private void Awake()
    {
        grid = new Building[GridSize.x, GridSize.y];
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(flyingBuilding != null)
        {
            var groundPlane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(groundPlane.Raycast(ray, out float position))
            {
                Vector3 worldPosition = ray.GetPoint(position);

                int x = Mathf.RoundToInt(worldPosition.x);
                int y = Mathf.RoundToInt(worldPosition.z);

                bool avaible = true;

                if (x < 0 || x > GridSize.x - flyingBuilding.Size.x) avaible = false;
                if (y < 0 || y > GridSize.y - flyingBuilding.Size.y) avaible = false;

                if (avaible && IsPlaceTaken(x, y)) avaible = false;

                flyingBuilding.transform.position = new Vector3(x,0,y);
                flyingBuilding.SetTransparent(avaible);

                if(avaible && Input.GetMouseButtonDown(0))
                {
                    PlaceFlyingBuilding(x, y);
                }
            }
        }
    }

    private bool IsPlaceTaken(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                if (grid[placeX + x, placeY + y] != null)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void PlaceFlyingBuilding(int placeX, int placeY)
    {
        for (int x = 0; x < flyingBuilding.Size.x; x++)
        {
            for (int y = 0; y < flyingBuilding.Size.y; y++)
            {
                grid[placeX + x, placeY + y] = flyingBuilding;
            }
        }
        SetBuildingFeatures();
    }

    private void SetBuildingFeatures()
    {
        flyingBuilding.SetNormal();
        MoneyManager.Instance.IncreaseMoneyPerMinute(flyingBuilding.GetMoneyIncreasing());
        MoneyManager.Instance.DecreaseMoney(flyingBuilding.GetBuildingPrice());
        flyingBuilding.transform.SetParent(gameObject.transform);

        placedBuildings.Add(flyingBuilding.GetId(), flyingBuilding);
       
        flyingBuilding = null;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if(MoneyManager.Instance.moneyAmount >= buildingPrefab.GetBuildingPrice())
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
            }

            flyingBuilding = Instantiate(buildingPrefab);
        }
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                if ((x + y) % 2 == 0)
                {
                    Gizmos.color = new Color(rFirst, gFirst, bFirst, aFirst);
                }
                else
                {
                    Gizmos.color = new Color(rSecond, gSecond, bSecond, aSecond);
                }

                Gizmos.DrawCube(transform.position + new Vector3(x, 0.1f, y), new Vector3(1, 0.1f, 1));
            }
        }
    }

    public void LoadData(GameData data)
    {
        foreach (var saveData in data.buildingsPlaced)
        {
            Building prefab = buildingPrefabs.Find(b => b.name == saveData.prefabName);
            if (prefab != null)
            {
                Building buildingInstance = Instantiate(prefab, saveData.position, Quaternion.identity);
                buildingInstance.id = saveData.id;
                buildingInstance.SetNormal();

                placedBuildings[buildingInstance.GetId()] = buildingInstance;
            }
        }
    }

    public void SaveData(ref GameData data)
    {
        data.buildingsPlaced.Clear();
        foreach (var building in placedBuildings.Values)
        {
            data.buildingsPlaced.Add(new BuildingDataSave(building));
        }
    }
}

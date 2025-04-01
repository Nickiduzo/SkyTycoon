using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour, IDataPersistence
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public List<Building> buildingPrefabs;
    public List<Building> rockPrefabs;

    private Dictionary<string, Building> placedBuildings = new Dictionary<string, Building>();
    private List<Building> placedRocks = new List<Building>();

    private Dictionary<string, string> requiredFactories = new Dictionary<string, string>
    {
        { "CarStore", "CarFactory" }, 
        { "AppliancesStore", "AppliancesFactory" },
        { "ChemicalStore", "ChemicalFactory" },
        { "ElectronicStore", "ElectronicFactory" },
        { "SoftwareStore", "SoftwareCompany" },
        { "VideoGamesStore", "StudioVideoGames" },
    };

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

    private void Start()
    {
        if (placedRocks.Count == 0)
        {
            GenerateRockBorders();
        }
    }

    private void GenerateRockBorders()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            PlaceRock(x, 0);
            PlaceRock(x, GridSize.y - 1);
        }

        for(int y = 1; y < GridSize.y - 1; y++)
        {
            PlaceRock(0, y);
            PlaceRock(GridSize.y - 1, y);
        }
    }

    private void PlaceRock(int x, int y)
    {
        int rockIndex = Random.Range(0, rockPrefabs.Count);
        Building rock = Instantiate(rockPrefabs[rockIndex], new Vector3(x, 0, y), Quaternion.identity);
        placedRocks.Add(rock);
        rock.transform.SetParent(gameObject.transform);
        grid[x, y] = rock;
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

                if (Input.GetKeyDown(KeyCode.Q))
                {
                    flyingBuilding.RotateBuilding(1);
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    flyingBuilding.RotateBuilding(-1);
                }

                if (avaible && Input.GetMouseButtonDown(0))
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
        flyingBuilding.PlayBuildEffect();
        flyingBuilding.SetNormal();
        MoneyManager.Instance.IncreaseMoneyPerMinute(flyingBuilding.GetMoneyIncreasing());
        MoneyManager.Instance.DecreaseMoney(flyingBuilding.GetBuildingPrice());
        flyingBuilding.transform.SetParent(gameObject.transform);

        placedBuildings.Add(flyingBuilding.GetId(), flyingBuilding);
       
        flyingBuilding = null;
    }

    public void StartPlacingBuilding(Building buildingPrefab)
    {
        if (!CanPlaceBuilding(buildingPrefab))
        {
            HintManager.Instance.NoSuitableFactoryAlert(buildingPrefab);
            return;
        }

        if(MoneyManager.Instance.moneyAmount >= buildingPrefab.GetBuildingPrice())
        {
            if (flyingBuilding != null)
            {
                Destroy(flyingBuilding.gameObject);
            }

            flyingBuilding = Instantiate(buildingPrefab);
        }
        else
        {
            HintManager.Instance.MoneyAlert();
        }
    }

    private bool CanPlaceBuilding(Building buildingPrefab)
    {
        if (requiredFactories.TryGetValue(buildingPrefab.name, out string requiredFactory))
        {
            return placedBuildings.Values.Any(building => building.name.Replace("(Clone)", "").Trim() == requiredFactory);
        }

        return true;
    }

    //private void OnDrawGizmos()
    //{
    //    for (int x = 0; x < GridSize.x; x++)
    //    {
    //        for (int y = 0; y < GridSize.y; y++)
    //        {
    //            if ((x + y) % 2 == 0)
    //            {
    //                Gizmos.color = new Color(rFirst, gFirst, bFirst, aFirst);
    //            }
    //            else
    //            {
    //                Gizmos.color = new Color(rSecond, gSecond, bSecond, aSecond);
    //            }

    //            Gizmos.DrawCube(transform.position + new Vector3(x, 0.1f, y), new Vector3(1, 0.1f, 1));
    //        }
    //    }
    //}

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
                buildingInstance.transform.SetParent(gameObject.transform);

                buildingInstance.currentRotation = saveData.buildingRotation;
                buildingInstance.RotateBuilding(buildingInstance.currentRotation);

                placedBuildings[buildingInstance.GetId()] = buildingInstance;

                int placeX = Mathf.RoundToInt(saveData.position.x);
                int placeY = Mathf.RoundToInt(saveData.position.z);

                for (int x = 0; x < buildingInstance.Size.x; x++)
                {
                    for (int y = 0; y < buildingInstance.Size.y; y++)
                    {
                        grid[placeX + x, placeY + y] = buildingInstance;
                    }
                }
            }
        }

        foreach (var rockData in data.rockPlaced)
        {
            Building prefab = rockPrefabs.Find(r => r.name == rockData.prefabName);
            if (prefab != null)
            {
                Building rockInstance = Instantiate(prefab, rockData.position, Quaternion.identity);
                rockInstance.id = rockData.id;
                rockInstance.SetNormal();
                rockInstance.transform.SetParent(gameObject.transform);

                int placeX = Mathf.RoundToInt(rockData.position.x);
                int placeY = Mathf.RoundToInt(rockData.position.z);

                for (int x = 0; x < rockInstance.Size.x; x++)
                {
                    for (int y = 0; y < rockInstance.Size.y; y++)
                    {
                        grid[placeX + x, placeY + y] = rockInstance;
                    }
                }
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

        data.rockPlaced.Clear();
        foreach (var rock in placedRocks)
        {
            data.rockPlaced.Add(new BuildingDataSave(rock));
        }
    }
}

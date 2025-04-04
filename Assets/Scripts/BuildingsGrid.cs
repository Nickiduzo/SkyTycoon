using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

public class BuildingsGrid : MonoBehaviour, IDataPersistence
{
    public Vector2Int GridSize = new Vector2Int(10, 10);
    public List<Building> buildingPrefabs;
    
    public List<Building> borderRocksPrefabs;
    private Dictionary<string, Building> placedRocks = new Dictionary<string, Building>();

    [SerializeField] private GameObject mainHall;

    [SerializeField] private List<Building> grassPrefabs;
    [SerializeField] private List<Building> rocksPrefabs;
    [SerializeField] private List<Building> treesPrefabs;
    private Dictionary<string, Building> rabishPlaced = new Dictionary<string, Building>();

    private Dictionary<string, Building> placedBuildings = new Dictionary<string, Building>();

    private Dictionary<string, string> requiredFactories = new Dictionary<string, string>
    {
        { "CarStore", "CarFactory" }, 
        { "AppliancesStore", "AppliancesFactory" },
        { "ChemicalStore", "ChemicalFactory" },
        { "ElectronicStore", "ElectronicFactory" },
        { "SoftwareStore", "SoftwareCompany" },
        { "VideoGamesStore", "StudioVideoGames" },
    };

    [SerializeField] private GameObject destructionEffect;

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

        Building.OnDelete += RemoveBuildings;
    }

    #region Landscape

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
        int rockIndex = Random.Range(0, borderRocksPrefabs.Count);
        Building rock = Instantiate(borderRocksPrefabs[rockIndex], new Vector3(x, 0, y), Quaternion.identity);
        rock.SetNormal();
        rock.transform.SetParent(gameObject.transform);
        placedRocks.Add(rock.GetId(), rock);
        grid[x, y] = rock;
    }

    private void GenerateRabish()
    {
        for (int x = 0; x < GridSize.x; x++)
        {
            for (int y = 0; y < GridSize.y; y++)
            {
                if(x != 0 && x != GridSize.x - 1 && y != 0 && y != GridSize.y - 1)
                {
                    int chooseRabish = Random.Range(0, 31);
                    Building prefab = null;

                    if (chooseRabish == 10 || chooseRabish == 5)
                    {
                        int grassIndex = Random.Range(0, grassPrefabs.Count);
                        prefab = grassPrefabs[grassIndex];
                    }
                    else if (chooseRabish == 20)
                    {
                        int treeIndex = Random.Range(0, treesPrefabs.Count);
                        prefab = treesPrefabs[treeIndex];
                    }
                    else if (chooseRabish == 30)
                    {
                        int rockIndex = Random.Range(0, rocksPrefabs.Count);
                        prefab = rocksPrefabs[rockIndex];
                    }

                    if (prefab != null && !IsPlaceTaken(prefab, x, y))
                    {
                        int rotateValue = Random.Range(0, 4);
                        Vector3 position = new Vector3(x, 0, y);
                        Building instance = Instantiate(prefab, position, Quaternion.identity);
                        instance.RotateBuilding(rotateValue);
                        instance.SetNormal();
                        instance.transform.SetParent(gameObject.transform);
                        rabishPlaced.Add(instance.GetId(), instance);
                        grid[x, y] = instance;
                    }
                }
            }
        }
    }

    #endregion

    private void GenerateHall()
    {
        int xMapCentre = (GridSize.x - 4) / 2;
        int zMapCentre = (GridSize.y - 4) / 2;
 
        Hall hall = Instantiate(mainHall, new Vector3(xMapCentre, 0, zMapCentre), Quaternion.identity).GetComponent<Hall>();

        if (hall != null)
        {
            
            hall.SetNormal();
            hall.transform.SetParent(gameObject.transform);
            hall.SetRotation(2);
            placedBuildings.Add(hall.GetId(), hall);

            for (int x = 0; x < hall.Size.x; x++)
            {
                for (int z = 0; z < hall.Size.y; z++)
                {
                    grid[xMapCentre + x, zMapCentre + z] = hall;
                }
            }
        }
    }

    private void Update()
    {
        if (flyingBuilding != null && Input.GetKeyDown(KeyCode.Escape))
        {
            Destroy(flyingBuilding.gameObject);
            flyingBuilding = null;
        }

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

    private bool IsPlaceTaken(Building building, int placeX, int placeY)
    {
        for (int x = 0; x < building.Size.x; x++)
        {
            for (int y = 0; y < building.Size.y; y++)
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
        AudioManager.Instance.Play("PlaceBuilding");
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
        if (buildingPrefab is Hall)
        {
            if (placedBuildings.Values.Any(h => h is Hall))
            {
                HintManager.Instance.HallPlacedAlert();
                return;
            }
        }

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

    private void RemoveBuildings(Building building)
    {
        if (building == null) return;

        string buildingId = building.GetId();

        if (rabishPlaced.ContainsKey(buildingId))
        {
            rabishPlaced.Remove(buildingId);
        }

        if (placedBuildings.ContainsKey(buildingId))
        {
            placedBuildings.Remove(buildingId);
        }

        Instantiate(destructionEffect, building.transform.position, Quaternion.identity);

        Destroy(building.gameObject);
    }

    public void LoadData(GameData data)
    {
        if (data.buildingsPlaced.Count == 0)
        {
            GenerateHall();
        }
        else
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

                    if(buildingInstance is Hall hall)
                    {
                        hall.currentTyre = data.hallTyre;
                    }

                    buildingInstance.SetRotation(saveData.buildingRotation);

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
        }

        if(data.borderRocksPlaced.Count == 0)
        {
            GenerateRockBorders();
        }
        else
        {
            foreach (var rockData in data.borderRocksPlaced)
            {
                Building prefab = borderRocksPrefabs.Find(r => r.name == rockData.prefabName);
                if (prefab != null)
                {
                    Building rockInstance = Instantiate(prefab, rockData.position, Quaternion.identity);
                    rockInstance.id = rockData.id;
                    rockInstance.SetNormal();
                    rockInstance.transform.SetParent(gameObject.transform);
                    placedRocks[rockInstance.GetId()] = rockInstance;

                    int placeX = Mathf.RoundToInt(rockData.position.x);
                    int placeY = Mathf.RoundToInt(rockData.position.z);

                    grid[placeX, placeY] = rockInstance;
                }
            }
        }

        if(data.rabishPlaced.Count == 0)
        {
            GenerateRabish();
        }
        else
        {
            foreach (var rabishData in data.rabishPlaced)
            {
                Building prefab = FindPrefabByName(rabishData.prefabName);
                if (prefab != null)
                {
                    Building instance = Instantiate(prefab, rabishData.position, Quaternion.identity);
                    instance.id = rabishData.id;
                    instance.SetNormal();
                    instance.transform.SetParent(gameObject.transform);
                    rabishPlaced[instance.GetId()] = instance;

                    instance.SetRotation(rabishData.buildingRotation);

                    int placeX = Mathf.RoundToInt(rabishData.position.x);
                    int placeY = Mathf.RoundToInt(rabishData.position.z);

                    grid[placeX, placeY] = instance;
                }
            }
        }
    }


    public void SaveData(ref GameData data)
    {
        data.buildingsPlaced.Clear();
        foreach (var building in placedBuildings.Values)
        {
            if(building is Hall hall)
            {
                data.hallTyre = hall.currentTyre;
                data.hallIncreaseFactor = hall.GetCurrentIncreaseFactor();
            }

            data.buildingsPlaced.Add(new BuildingDataSave(building));
        }

        data.borderRocksPlaced.Clear();
        foreach (var rock in placedRocks.Values)
        {
            data.borderRocksPlaced.Add(new BuildingDataSave(rock));
        }

        data.rabishPlaced.Clear();
        foreach (var rabish in rabishPlaced.Values)
        {
            data.rabishPlaced.Add(new BuildingDataSave(rabish));
        }
    }

    private Building FindPrefabByName(string prefabName)
    {
        Building prefab = grassPrefabs.Find(g => g.name == prefabName);
        if (prefab == null) prefab = treesPrefabs.Find(t => t.name == prefabName);
        if (prefab == null) prefab = rocksPrefabs.Find(r => r.name == prefabName);
        return prefab;
    }

    private void OnDisable()
    {
        Building.OnDelete -= RemoveBuildings;
    }
}

using UnityEngine;

public class Building : MonoBehaviour
{
    [HideInInspector] public string id;

    [SerializeField] protected BuildingData BuildingData;

    [SerializeField] private ParticleSystem buildEffect;
    
    public Renderer MainRenderer;
    public Vector2Int Size = Vector2Int.one;

    public Vector3 position;

    public int currentRotation;
    public BuildingRotation[] buildingRotations;
    public GameObject buildingModel;

    protected bool isPlaced = false;

    public void SetTransparent(bool avaible)
    {
        if (avaible)
        {
            MainRenderer.material.color = Color.green;
        }
        else
        {
            MainRenderer.material.color = Color.red;
        }
    }

    public virtual void SetNormal()
    {
        MainRenderer.materials = BuildingData.GetMaterials();
        isPlaced = true;
        position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        GenerateGuid();
    }

    public void RotateBuilding(int value)
    {
        currentRotation += value;

        if (currentRotation >= buildingRotations.Length)
        {
            currentRotation = 0;
        }

        if (currentRotation < 0)
        {
            currentRotation = buildingRotations.Length - 1;
        }

        buildingModel.transform.localPosition = buildingRotations[currentRotation].buildingPosition;
        buildingModel.transform.localRotation = Quaternion.Euler(buildingRotations[currentRotation].buildingRotation);

        Size = buildingRotations[currentRotation].rotationSize;
    }

    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }
    
    public void PlayBuildEffect()
    {
        buildEffect.Play();
    }

    public float GetBuildingPrice()
    {
        return BuildingData.Price;
    }

    public float GetMoneyIncreasing()
    {
        return BuildingData.MoneyPerMin;
    }
    public string GetId()
    {
        return id;
    }

    private void OnDrawGizmos()
    {
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                if((x + y) % 2 == 0)
                {
                    Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                }
                else
                {
                    Gizmos.color = new Color(1f, 0.68f, 0f, 0.3f);
                }

                Gizmos.DrawCube(transform.position + new Vector3(x, 0.1f, y), new Vector3(1, 0.1f, 1));
            }
        }
    }
}

[System.Serializable]
public class BuildingRotation
{
    public string rotationName;
    public Vector3 buildingPosition;
    public Vector3 buildingRotation;
    public Vector2Int rotationSize;
}

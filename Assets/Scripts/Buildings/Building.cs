using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    public static event Action<Building> OnDelete;

    [Header("Information")]
    [SerializeField] protected BuildingData BuildingData;
    [SerializeField] private ParticleSystem buildEffect;
    [HideInInspector] public string id;

    [Header("Rotation")]
    public BuildingRotation[] buildingRotations;
    public int currentRotation;

    [Header("Position")]
    public Vector2Int Size = Vector2Int.one;
    public Vector3 position;
    protected bool isPlaced = false;

    [Header("Building Data")]
    [SerializeField] private GameObject buildingModel;
    [SerializeField] private Renderer MainRenderer;

    [Header("Building Interface")]
    [SerializeField] private GameObject uiInterface;
    [SerializeField] private TextMeshProUGUI buildingTitle;
    [SerializeField] private TextMeshProUGUI priceForRemove;
    [SerializeField] private Button removeBuildingButton;
    [SerializeField] private Button closeButton;
    
    private const float doubleClickThreshold = 0.3f;
    private float lastClickTime = 0f;

    private void Start()
    {
        if (removeBuildingButton != null)
        {
            removeBuildingButton.onClick.AddListener(RemoveBuilding);
        }

        if (closeButton != null)
        {
            closeButton.onClick.AddListener(CloseInterface);
        }
    }

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

    public void SetRotation(int value)
    {
        if(value >= 0 && value < buildingRotations.Length)
        {
            currentRotation = value;

            buildingModel.transform.localPosition = buildingRotations[currentRotation].buildingPosition;
            buildingModel.transform.localRotation = Quaternion.Euler(buildingRotations[currentRotation].buildingRotation);

            Size = buildingRotations[currentRotation].rotationSize;
        }
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


    #region UI

    public void CloseInterface()
    {
        UIPanelManager.Instance.panelIsActive = false;
        AudioManager.Instance.Play("ButtonClick");
        uiInterface.SetActive(false);
    }

    public virtual void OpenBuildingUI()
    {
        buildingTitle.text = BuildingData.Name.ToString();
        if (priceForRemove != null)
        {
            priceForRemove.text = BuildingData.RemovePrice.ToString() + " $";
        }
        uiInterface.SetActive(true);        
    }

    private void RemoveBuilding()
    {
        UIPanelManager.Instance.panelIsActive = false;
        AudioManager.Instance.Play("ButtonClick");
        uiInterface.SetActive(false);
        OnDelete?.Invoke(this);
    }

    private void OnMouseDown()
    {
        if (Time.time - lastClickTime < doubleClickThreshold && UIPanelManager.Instance.IsClosePanels() && isPlaced)
        {
            UIPanelManager.Instance.panelIsActive = true;
            AudioManager.Instance.Play("BuildingSelect");
            OpenBuildingUI();
        }
        lastClickTime = Time.time;
    }

    private void OnDestroy()
    {
        if (removeBuildingButton != null)
        {
            removeBuildingButton.onClick.RemoveListener(RemoveBuilding);
        }

        if (closeButton != null)
        {
            closeButton.onClick.RemoveListener(CloseInterface);
        }
    }

    #endregion

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

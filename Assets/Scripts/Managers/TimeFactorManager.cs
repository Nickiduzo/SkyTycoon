using UnityEngine;

public class TimeFactorManager : MonoBehaviour, IDataPersistence
{
    public static TimeFactorManager Instance;

    private float increaseFactor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }

    public void LoadData(GameData data)
    {
        increaseFactor = data.hallIncreaseFactor;
    }

    public void SaveData(ref GameData data)
    {
        // here could be another realization    
    }


    public float GetBuildingMaxTime(float defaultValue)
    {
        return defaultValue * increaseFactor;
    }
}

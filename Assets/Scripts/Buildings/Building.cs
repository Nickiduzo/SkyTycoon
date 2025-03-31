using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] protected BuildingData BuildingData;
    
    public Renderer MainRenderer;
    public Vector2Int Size = Vector2Int.one;

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
    }

    public float GetBuildingPrice()
    {
        return BuildingData.Price;
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

using UnityEngine;

public class Building : MonoBehaviour
{
    public Renderer[] MainRenderer;
    public Vector2Int Size = Vector2Int.one;

    public void SetTransparent(bool avaible)
    {
        if (avaible)
        {
            for (int i = 0; i < MainRenderer.Length; i++)
            {
                MainRenderer[i].material.color = Color.green;
            }
        }
        else
        {
            for (int i = 0; i < MainRenderer.Length; i++)
            {
                MainRenderer[i].material.color = Color.red;
            }
        }
    }

    public void SetNormal()
    {
        for (int i = 0; i < MainRenderer.Length; i++)
        {
            MainRenderer[i].material.color = Color.white;
        }
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

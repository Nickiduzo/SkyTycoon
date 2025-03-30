using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;

    private int width;
    private int height;

    [SerializeField] private float boundary;


    private void Awake()
    {
        width = Screen.width;
        height = Screen.height;
    }

    private void LateUpdate()
    {
        MoveCamera();
    }

    private void MoveCamera()
    {
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                           0.0f, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
            }

            else if (Input.GetAxis("Mouse X") < 0)
            {
                transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                           0.0f, Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
            }
        }

        if (Input.mousePosition.x > width - boundary)
        {
            transform.position -= new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                       0.0f, 0.0f);
        }

        if (Input.mousePosition.x < 0 + boundary)
        {
            transform.position -= new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * speed,
                                       0.0f, 0.0f);
        }

        if (Input.mousePosition.y > height - boundary)
        {
            transform.position -= new Vector3(0.0f, 0.0f,
                                       Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
        }

        if (Input.mousePosition.y < 0 + boundary)
        {
            transform.position -= new Vector3(0.0f, 0.0f,
                                       Input.GetAxisRaw("Mouse Y") * Time.deltaTime * speed);
        }
    }
}

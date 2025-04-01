using UnityEngine;

public class CameraController : MonoBehaviour, IDataPersistence
{
    [Header("Change Camera Position")]
    [SerializeField] private float speed = 10f;

    [SerializeField] private float xRightLimit = 0;
    [SerializeField] private float xLeftLimit = 0;

    [SerializeField] private float zRightLimit = 0;
    [SerializeField] private float zLeftLimit = 0;

    [Header("Change Camera Zoom")]
    [SerializeField] private float zoomSpeed = 6f;
    [SerializeField] private float zoomSmoothness = 5f;

    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 40f;

    [SerializeField] private float currentZoom;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();
        SetLimits();
    }

    private void SetLimits()
    {
        if (transform.position.x > xRightLimit)
        {
            transform.position = new Vector3(xRightLimit, transform.position.y, transform.position.z);
        }

        if (transform.position.x < xLeftLimit)
        {
            transform.position = new Vector3(xLeftLimit, transform.position.y, transform.position.z);
        }

        if (transform.position.z > zRightLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zRightLimit);
        }

        if (transform.position.z < zLeftLimit)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zLeftLimit);
        }
    }

    private void MoveCamera()
    {
        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");

        if(Input.GetMouseButton(1))
        {
            transform.position += new Vector3(-horizontal, 0, -vertical) * speed * Time.deltaTime;
        }
    }

    private void ZoomCamera()
    {
        currentZoom = Mathf.Clamp(currentZoom - Input.mouseScrollDelta.y * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
        mainCamera.orthographicSize = Mathf.Lerp(mainCamera.orthographicSize, currentZoom, zoomSmoothness * Time.deltaTime);
    }

    public void LoadData(GameData data)
    {
        transform.position = data.cameraPosition;
    }

    public void SaveData(ref GameData data)
    {
        data.cameraPosition = transform.position;
    }
}

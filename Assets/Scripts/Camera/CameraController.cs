using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class CameraController : MonoBehaviour, IDataPersistence
{
    [Header("Change Camera Position")]
    [SerializeField] private float speed = 10f;

    [Header("Bounds")]
    [SerializeField] private float east;
    [SerializeField] private float west;
    [SerializeField] private float north;
    [SerializeField] private float south;

    [Header("Change Camera Zoom")]
    [SerializeField] private float zoomSpeed = 6f;
    [SerializeField] private float zoomSmoothness = 5f;

    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 40f;

    [SerializeField] private float currentZoom;

    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
        MenuUI.OnChangeScroll += SetSpeed;
    }

    private void LateUpdate()
    {
        ZoomCamera();
        MoveCamera();

        ClampCameraPosition();
    }

    private void ClampCameraPosition()
    {
        float camSize = mainCamera.orthographicSize;
        float camWidth = camSize * mainCamera.aspect;

        float minX = west + camWidth;
        float maxX = east - camWidth;
        float minY = south + camSize;
        float maxY = north - camSize;

        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.z = Mathf.Clamp(clampedPosition.z, minY, maxY);

        transform.position = clampedPosition;
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

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void LoadData(GameData data)
    {
        transform.position = data.cameraPosition;
        speed = data.scrollSliderValue;
    }

    public void SaveData(ref GameData data)
    {
        data.cameraPosition = transform.position;
        data.scrollSliderValue = speed;
    }

    private void OnDisable()
    {
        MenuUI.OnChangeScroll -= SetSpeed;
    }
}

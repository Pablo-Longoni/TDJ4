using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CameraZoom : MonoBehaviour
{
    public float _maxZoom = 20;
    public float _minZoom = 5;

    public float _zoomSpeed = 5;
    public float _zoomSmoothness = 1;

    private float _currentZoom;

    public CinemachineCamera _cinemachineCamera;

    void Start()
    {
    }

    private void Awake()
    {
        //  _cinemachineCamera = GetComponent<CinemachineCamera>();

        // Obtener la c�mara principal asociada
        //  _mainCamera = Camera.main;

        // Inicializar  zoom con el tama�o ortogr�fico actual de la c�mara
        _currentZoom = _cinemachineCamera.Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {

        bool zoomIn = Input.GetKey("joystick button 6") || Input.mouseScrollDelta.y > 0f;
        bool zoomOut = Input.GetKey("joystick button 7") || Input.mouseScrollDelta.y < 0f;

        if (zoomIn)
        {
            _currentZoom -= _zoomSpeed * Time.deltaTime;
        }

        if (zoomOut)
        {
            _currentZoom += _zoomSpeed * Time.deltaTime;
        }

        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

        _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(
            _cinemachineCamera.Lens.OrthographicSize,
            _currentZoom,
            _zoomSmoothness * Time.deltaTime
        );
    }
}

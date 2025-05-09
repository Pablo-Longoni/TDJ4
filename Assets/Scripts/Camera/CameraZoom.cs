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

        // Obtener la cámara principal asociada
        //  _mainCamera = Camera.main;

        // Inicializar  zoom con el tamaño ortográfico actual de la cámara
        _currentZoom = _cinemachineCamera.Lens.OrthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        _currentZoom = Mathf.Clamp(_currentZoom - Input.mouseScrollDelta.y * _zoomSpeed * Time.deltaTime, _minZoom, _maxZoom);
   //   _mainCamera.orthographicSize = Mathf.Lerp(_mainCamera.orthographicSize, _currentZoom,_zoomSmoothness * Time.deltaTime);
        _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(_cinemachineCamera.Lens.OrthographicSize, _currentZoom, _zoomSmoothness * Time.deltaTime);
    }
}

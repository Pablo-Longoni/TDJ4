using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    public float _maxZoom = 20;
    public float _minZoom = 5;
    public float _zoomSpeed = 5;
    public float _zoomSmoothness = 1;

    private float _currentZoom;

    public CinemachineCamera _cinemachineCamera;

    private PlayerInputReader _input;

    private void Awake()
    {
        _currentZoom = _cinemachineCamera.Lens.OrthographicSize;
        _input = FindObjectOfType<PlayerInputReader>();
    }

    void Update()
    {
        if (_input == null) return;

        
        float scrollValue = Mouse.current?.scroll.ReadValue().y ?? 0f;

        bool zoomIn = _input.ZoomInHeld || scrollValue > 0f;
        bool zoomOut = _input.ZoomOutHeld || scrollValue < 0f;

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

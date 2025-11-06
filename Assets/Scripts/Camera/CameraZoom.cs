using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoom : MonoBehaviour
{
    [Header("Zoom Limits")]
    public float _maxZoom = 20;
    public float _minZoom = 5;

    [Header("Mouse/Keyboard Zoom")]
    public float _zoomSpeed = 5;

    [Header("Pinch Zoom")]
    public float pinchZoomSpeed = 4f; // Separate speed for pinch
    [Range(0.001f, 0.1f)]
    public float pinchSensitivity = 0.01f;
    public bool normalizeByScreen = true;

    [Header("Smoothness")]
    public float _zoomSmoothness = 1;

    private float _currentZoom;
    private float _lastPinchDistance;

    public CinemachineCamera _cinemachineCamera;
    private PlayerInputReader _input;

    private void Awake()
    {
        _currentZoom = _cinemachineCamera.Lens.OrthographicSize;
        _input = FindFirstObjectByType<PlayerInputReader>();
    }

    private void Start()
    {
        _zoomSpeed = PlayerPrefs.GetFloat("ZoomSpeed", 160);
    }

    void Update()
    {
        if (_input == null) return;

        // Zoom con rueda del mouse y teclas
        bool zoomIn = _input.ZoomInHeld || Input.mouseScrollDelta.y > 0f;
        bool zoomOut = _input.ZoomOutHeld || Input.mouseScrollDelta.y < 0f;

        // Pinch Zoom táctil
        HandlePinchZoom();

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

    private void HandlePinchZoom()
    {
        if (_input.IsPinching)
        {
            Vector2 pos0 = _input.Touch0Position;
            Vector2 pos1 = _input.Touch1Position;

            float currentDist = Vector2.Distance(pos0, pos1);

            if (_lastPinchDistance > 0)
            {
                float pinchDelta = currentDist - _lastPinchDistance;

                if (normalizeByScreen)
                {
                    float screenDiag = Mathf.Sqrt(Screen.width * Screen.width + Screen.height * Screen.height);
                    pinchDelta /= screenDiag;
                }

                // Use separate pinch speed with frame-rate independence
                _currentZoom -= pinchDelta * pinchZoomSpeed * Time.deltaTime / pinchSensitivity;
            }

            _lastPinchDistance = currentDist;
        }
        else
        {
            _lastPinchDistance = 0;
        }
    }

    public void SetZoomSpeed(float newSpeed)
    {
        _zoomSpeed = newSpeed;
    }
}
using System.Collections;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEngine;
public class DoorTrigger : MonoBehaviour
{
    [Header("Portal settings")]
    [SerializeField] private Transform _portal;
    [SerializeField] private GameObject _target;
    [SerializeField] private BoxCollider _portalCollider;

    [Header("Camera settings")]
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private CameraZoom _cameraZoom;
    [SerializeField] private CameraChange _cameraChange;
    [SerializeField] private float _zoomChange = -20;
    [SerializeField] private float zoomDuration = 1.5f;
    [SerializeField] private float _moveDistance = 200f;
    [SerializeField] private CinemachineCamera _cinemachineCamera;
    [SerializeField] private float _minZoom = 5;
    [SerializeField] private float _maxZoom = 150;


    [Header("Player settings")]
    [SerializeField] private Vector3 _offSetPlayerEnter = new Vector3(0, 2, 0);
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private CubeAnimation _cubeAnimation;
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody _rb;

    [Header("Dust settings")]
    [SerializeField] private ParticleSystem _particleEnterPortal;
    private Quaternion _rotationParticle = Quaternion.Euler (-90, 0, 0);
    private Vector3 _offSet = new Vector3 (0, -2, 0);

    [Header("Others")]
    private ChangeScene _changeScene;

    [Header("Vibration Settings")]
    [SerializeField] private float vibrationIntensity = 0.8f;
    [SerializeField] private float vibrationDuration = 2f;


    // XInput para Windows (Logitech F710 en modo X)
    [DllImport("XInput1_4.dll")]
    private static extern uint XInputSetState(uint dwUserIndex, ref XInputVibration pVibration);

    [StructLayout(LayoutKind.Sequential)]
    private struct XInputVibration
    {
        public ushort wLeftMotorSpeed;
        public ushort wRightMotorSpeed;
    }

    void Start()
    {
        _changeScene = GameObject.FindGameObjectWithTag("GameController").GetComponent<ChangeScene>();
        _rb = _player.GetComponent<Rigidbody>();


    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric)
        {
            _portalCollider.enabled = false;

            _rb.isKinematic = true;

            StartCoroutine(EnterPortalSequence());

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._portal);
            }
            else
            {
                return;
            }
        }
    }

    private IEnumerator EnterPortalSequence()
    {
        _player.enabled = false;
        _cubeAnimation.IgnoreStretchAndSquash(2);

        if (_cameraZoom != null)
            _cameraZoom.enabled = false;

        yield return StartCoroutine(ZoomIn());
    }


    private IEnumerator MovePlayerToDoor(Vector3 targetPosition)
    {
        StartCoroutine(VibrateGamepadXInput(vibrationIntensity, vibrationDuration));

        _cubeAnimation.EnterPortalAnim(0.5f);

        Vector3 initialPosition = _player.transform.position;

        Instantiate(_particleEnterPortal, targetPosition + _offSet, _rotationParticle);

        _cameraShake.Shake(2f, 2, .4f);

        float duration = 1f;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;

            float t = Mathf.Clamp01(timeElapsed / duration);

            _player.transform.position = Vector3.Lerp(initialPosition, targetPosition + _offSetPlayerEnter, t);

            yield return null;
        }

        _player.transform.position = targetPosition + _offSetPlayerEnter;
        _player.gameObject.SetActive(false);
    }

    public IEnumerator ZoomIn()
    {
        Vector3 startPos = _cinemachineCamera.transform.position;

        float startSize = _cinemachineCamera.Lens.OrthographicSize;

        Vector3 targetPos = startPos + (_portal.position - startPos).normalized * _moveDistance;

        float targetSize = Mathf.Clamp(startSize + _zoomChange ,_minZoom, _maxZoom);

        _cinemachineCamera.Target.TrackingTarget = _portal;

        StartCoroutine(MovePlayerToDoor(_target.transform.position));

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / zoomDuration;
            t = Mathf.Clamp01(t);

            _cinemachineCamera.transform.position =
                Vector3.Lerp(startPos, targetPos, t);

            _cinemachineCamera.Lens.OrthographicSize =
                Mathf.Lerp(startSize, targetSize, t);

            yield return null;
        }

        _changeScene.NextLevel();

        _cinemachineCamera.transform.position = targetPos;
        _cinemachineCamera.Lens.OrthographicSize = targetSize;
    }

    // Método usando XInput directamente (para Logitech F710)
    private IEnumerator VibrateGamepadXInput(float intensity, float duration)
    {
        // Convertir intensidad (0-1) a rango de XInput (0-65535)
        ushort motorSpeed = (ushort)(intensity * 65535);

        XInputVibration vibration = new XInputVibration
        {
            wLeftMotorSpeed = motorSpeed,
            wRightMotorSpeed = motorSpeed
        };

        // Activar vibración (índice 0 = primer control)
        uint result = XInputSetState(0, ref vibration);

        if (result == 0)
        {
            Debug.Log($"[XInput] Vibración activada - Intensidad: {intensity}");
        }
        else
        {
            Debug.LogWarning($"[XInput] Error al activar vibración: {result}");
        }

        // Esperar la duración
        yield return new WaitForSecondsRealtime(duration);

        // Detener vibración
        vibration.wLeftMotorSpeed = 0;
        vibration.wRightMotorSpeed = 0;
        XInputSetState(0, ref vibration);

        // Debug.Log("[XInput] Vibración detenida");
    }

    private void OnDestroy()
    {
        StopVibrationXInput();
    }

    public void StopVibrationXInput()
    {
        XInputVibration vibration = new XInputVibration
        {
            wLeftMotorSpeed = 0,
            wRightMotorSpeed = 0
        };
        XInputSetState(0, ref vibration);
    }
}
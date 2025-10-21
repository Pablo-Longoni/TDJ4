using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
public class DoorTrigger : MonoBehaviour
{
    private ChangeScene _changeScene;
    public CameraChange _cameraChange;
    [SerializeField] public PlayerMovement _player;
    [SerializeField] public CubeAnimation _cubeAnimation;
    public float moveSpeed = 5f;
    public GameObject _target;
    [SerializeField] public AudioManager _audioManager;
    string _sceneName = string.Empty;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] public CameraZoom _cameraZoom;
    [SerializeField] private ParticleSystem _particleEnterPortal;



    [Header("Dolly effect Settings")]
    public Transform _portal;
    public float _originalSize = 35;
    public float _zoomChange = -20;
    public float zoomDuration = 1.5f;
    public Vector3 _originalPos;
    [SerializeField] private float _moveDistance = 200f;
    public CinemachineCamera _cinemachineCamera;
    public float _minZoom;
    public float _maxZoom;
    public Rigidbody _rb;
    public float levitateDistance = 2;
    private Quaternion _rotationParticle = Quaternion.Euler(-90, 0, 0);

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
        _audioManager = FindFirstObjectByType<AudioManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric)
        {

            _rb.isKinematic = true;
            //    StartCoroutine(MovePlayerToDoor(_target.transform.position));
            StartCoroutine(EnterPortalSequence());
           // AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._portal);
       //     _audioManager.DistoredMusic();
            Debug.Log("Jugador entró en la puerta");
        }
    }

    private IEnumerator EnterPortalSequence()
    {
        _player.enabled = false;
        _cubeAnimation.IgnoreStretchAndSquash(2);

        yield return StartCoroutine(ZoomIn());

        yield return StartCoroutine(MovePlayerToDoor(_target.transform.position));

        yield return StartCoroutine(ReturnToOriginal());
    }

    private IEnumerator MovePlayerToDoor(Vector3 targetPosition)
    {
        StartCoroutine(VibrateGamepadXInput(vibrationIntensity, vibrationDuration));
        _cubeAnimation.EnterPortalAnim();
        float timeElapsed = 0f;
        Vector3 initialPosition = _player.transform.position;
        Vector3 _offSet = new Vector3(0, -2, 0); 
        Instantiate(_particleEnterPortal, targetPosition + _offSet, _rotationParticle); 
        _cameraShake.Shake(2f, 2, .4f);

        //_audioManager.soundSource.PlayOneShot(_audioManager._portal);
        while (timeElapsed < 2f)
        {
            timeElapsed += Time.deltaTime * moveSpeed; 
            _player.transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed);
            yield return null;
        }
    }

    public IEnumerator ZoomIn()
    {

        _cinemachineCamera.Target.TrackingTarget = _portal;
        _originalSize = _cinemachineCamera.Lens.OrthographicSize;

        Vector3 levitateDirection = _target.transform.up; 
        Vector3 startPosLevitate = _player.transform.position;
        Vector3 targetPosLevitate = startPosLevitate + levitateDirection * levitateDistance;

        Vector3 startPos = _originalPos;
        Vector3 targetPos = startPos + (_portal.position - startPos).normalized * _moveDistance;
        float startSize = _originalSize;
        float targetSize = _originalSize + _zoomChange;

        targetSize = Mathf.Clamp(targetSize, _minZoom, _maxZoom);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomDuration;

            _cinemachineCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);

            _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(startSize, targetSize, t);

            _player.transform.position = Vector3.Lerp(startPosLevitate, targetPosLevitate, t);
            yield return null;
        }
    }

    public IEnumerator ReturnToOriginal()
    {
        Vector3 currentPos = _cinemachineCamera.transform.position;
        float currentSize = _cinemachineCamera.Lens.OrthographicSize;
        _changeScene.NextLevel();
      //  _audioManager.RestoredMusic();
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / zoomDuration;
            _cinemachineCamera.transform.position = Vector3.Lerp(currentPos, _originalPos, t);
            _cinemachineCamera.Lens.OrthographicSize = Mathf.Lerp(currentSize, _originalSize, t);
            yield return null;
        }
        // _cinemachineCamera.Target.TrackingTarget = null; 
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
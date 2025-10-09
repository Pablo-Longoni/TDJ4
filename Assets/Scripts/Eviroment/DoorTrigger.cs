using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Runtime.InteropServices;

public class DoorTrigger : MonoBehaviour
{
    private ChangeScene _changeScene;
    public CameraChange _cameraChange;
    [SerializeField] public PlayerMovement _player;
    [SerializeField] public CubeAnimation _cubeAnimation;
    public float moveSpeed = .7f;
    public GameObject _target;
    [SerializeField] public AudioManager _audioManager;
    string _sceneName = string.Empty;
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private CameraZoom _cameraZoom;
    [SerializeField] private ParticleSystem _particleEnterPortal;

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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _cameraChange._isIsometric)
        {
            _cameraZoom.ZoomIn(1500);
            StartCoroutine(MovePlayerToDoor(_target.transform.position));
            AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._portal);

            Debug.Log("Jugador entró en la puerta");
        }
    }

    private IEnumerator MovePlayerToDoor(Vector3 targetPosition)
    {
        _player.enabled = false;
        _cameraShake.Shake(2, 1, 1);

        // Iniciar vibración
        StartCoroutine(VibrateGamepadXInput(vibrationIntensity, vibrationDuration));

        float timeElapsed = 0f;
        Vector3 initialPosition = _player.transform.position;
        _cubeAnimation.EnterPortalAnim();
        Instantiate(_particleEnterPortal, targetPosition, Quaternion.identity);

        while (timeElapsed < 2f)
        {
            timeElapsed += Time.deltaTime * moveSpeed;
            _player.transform.position = Vector3.Lerp(initialPosition, targetPosition, timeElapsed);
            yield return null;
        }

        _changeScene.NextLevel();
        Debug.Log("Jugador entró en la puerta");
        _player.enabled = true;
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

        Debug.Log("[XInput] Vibración detenida");
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
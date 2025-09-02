using Unity.Cinemachine;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class CameraShake : MonoBehaviour
{
    private CinemachineCamera _cam;
    private CinemachineBasicMultiChannelPerlin _noise;

    private void Awake()
    {
        _cam = GetComponent<CinemachineCamera>();
        _noise = _cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        _noise.AmplitudeGain = 0;
        _noise.FrequencyGain = 0;
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        StartCoroutine(ShakeCoroutine(amplitude, frequency, duration));
    }

    private IEnumerator ShakeCoroutine(float amplitude, float frequency, float duration)
    {
        // 👇 activa cámara shake
        _noise.AmplitudeGain = amplitude;
        _noise.FrequencyGain = frequency;

        Debug.Log("Camera shake");


        if (Gamepad.current != null)
        {

            Gamepad.current.SetMotorSpeeds(frequency, amplitude);
        }

        yield return new WaitForSeconds(duration);


        _noise.AmplitudeGain = 0;
        _noise.FrequencyGain = 0;


        if (Gamepad.current != null)
        {
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}

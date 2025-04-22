using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using Unity.Cinemachine;
public class CameraMovement : MonoBehaviour
{
  /*  public static CameraMovement Instance;

    private CinemachineCamera  _camera;
    private CinemachineBasicMultiChannelPerlin _perlin;
    private float _movementTime;
    private float _totalMovementTime;
    private float _initIntensity;

    private void Start()
    {
        _perlin.AmplitudeGain = 0f;
        _perlin.FrequencyGain = 0f;
        _movementTime = 0f;
    }
    private void Awake()
    {
        Instance = this;
        _camera = GetComponent<CinemachineCamera>();
        _perlin = _camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void MoveCamera(float intensity, float frequency, float time)
    {
        _perlin.AmplitudeGain = intensity;
        _perlin.FrequencyGain = frequency;
        _initIntensity = intensity;
        _totalMovementTime = time;
        _movementTime = time;
    }

    private void Update()
    {
        if (_movementTime > 0)
        {
            _movementTime -= Time.deltaTime;
            float t = 1 - (_movementTime / _totalMovementTime);
            _perlin.AmplitudeGain = Mathf.Lerp(_initIntensity, 0, t);
        }
        else if (_perlin.AmplitudeGain != 0)
        {
            _perlin.AmplitudeGain = 0; // Asegurarse de que quede en 0
        }
    }*/
}

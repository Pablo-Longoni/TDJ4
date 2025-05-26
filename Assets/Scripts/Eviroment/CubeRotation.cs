using UnityEngine;
using System.Collections;

public class CubeRotation : MonoBehaviour
{
  private Quaternion _targetRotation;
  private float rotationSpeed = 200f;
  public bool _shouldRotate = false;
  public float _rotationTurn = 90f;
  public bool _canRotate = true;
  public float _rotateCooldown = 2f;
  
  //Blink figura
  public MeshRenderer _renderer;
  private Coroutine _blinkCoroutine;
  private float _fadeSpeed = 0.6f;
  private Color _originalColor;
  private Color _targetColor;

  [SerializeField] public AudioManager _audioManager;

  public bool _isInCooldown = false;
  public CameraChange _cameraChange;

  void Start()
  {
        _originalColor = _renderer.material.color;
        _targetColor = _originalColor * 0.8f;
    }
  void Update()
  {
    if (_shouldRotate)
    {
      transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, rotationSpeed * Time.deltaTime);

      if (Quaternion.Angle(transform.rotation, _targetRotation) < 0.1f)
      {
        transform.rotation = _targetRotation;
        _shouldRotate = false;
      }
    }
  }

  public void RotateCube(Vector3 rotationAxis, Transform player)
  {
    if (_canRotate && !_shouldRotate && !_isInCooldown)
    {
      // Calcular la nueva rotación objetivo
      Quaternion newTargetRotation = Quaternion.AngleAxis(_rotationTurn, rotationAxis) * transform.rotation;

      // Solo rotar si hay una diferencia real
      if (Quaternion.Angle(transform.rotation, newTargetRotation) > 0.1f)
      {
        _targetRotation = newTargetRotation;
        _shouldRotate = true;

        // Reproducir sonido y comenzar cooldown
        _audioManager.playSound(_audioManager._turning);
        StartCoroutine(RotationCooldown());
      }
    }
  }

  private IEnumerator RotationCooldown()
  {
    _isInCooldown = true;
    _canRotate = false;
    _cameraChange._canChange = false;
    yield return new WaitForSeconds(_rotateCooldown);

    _canRotate = true;
    _isInCooldown = false;
    _cameraChange._canChange = true;
    }


    public void StartBlinking()
    {
        if (_blinkCoroutine == null)
        {
            _blinkCoroutine = StartCoroutine(Blink());
        }
    }

    public void StopBlinking()
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
            _renderer.material.color = _originalColor; 
        }
    }


    private IEnumerator Blink()
    {
        float t = 0f;
        bool fadingToTarget = true;

        while (true)
        {
            t += Time.deltaTime * _fadeSpeed;

            if (fadingToTarget)
            {
                _renderer.material.color = Color.Lerp(_originalColor, _targetColor, t);
                if (t >= 1f)
                {
                    t = 0f;
                    fadingToTarget = false;
                }
            }
            else
            {
                _renderer.material.color = Color.Lerp(_targetColor, _originalColor, t);
                if (t >= 1f)
                {
                    t = 0f;
                    fadingToTarget = true;
                }
            }

            yield return null;
        }
    }
}

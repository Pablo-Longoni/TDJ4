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

    // Blink figura
    private MeshRenderer[] _renderers;
    private Color[] _originalColors;
    private Coroutine _blinkCoroutine;
    private float _fadeSpeed = 0.6f;
    private Color _targetColor;

    [SerializeField] public AudioManager _audioManager;

    public bool _isInCooldown = false;
    public CameraChange _cameraChange;

    void Start()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _originalColors = new Color[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _originalColors[i] = _renderers[i].material.color;
        }

        _targetColor = _originalColors[0] * 0.6f;
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

            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            if (horizontal < -0.5f)
            {
                RotateCube(Vector3.up, transform);
            }
            else if (horizontal > 0.5f)
            {
                RotateCube(Vector3.down, transform);
            }
            else if (vertical > 0.5f)
            {
                RotateCube(Vector3.right, transform);
            }
            else if (vertical < -0.5f)
            {
                RotateCube(Vector3.left, transform);
            }
        }
    }

    public void RotateCube(Vector3 rotationAxis, Transform player)
    {
        if (_canRotate && !_shouldRotate && !_isInCooldown)
        {
            Quaternion newTargetRotation = Quaternion.AngleAxis(_rotationTurn, rotationAxis) * transform.rotation;

            if (Quaternion.Angle(transform.rotation, newTargetRotation) > 0.1f)
            {
                _targetRotation = newTargetRotation;
                _shouldRotate = true;

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

            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material.color = _originalColors[i];
            }
        }
    }

    private IEnumerator Blink()
    {
        float t = 0f;
        bool fadingToTarget = true;

        while (true)
        {
            t += Time.deltaTime * _fadeSpeed;

            for (int i = 0; i < _renderers.Length; i++)
            {
                if (fadingToTarget)
                {
                    _renderers[i].material.color = Color.Lerp(_originalColors[i], _targetColor, t);
                }
                else
                {
                    _renderers[i].material.color = Color.Lerp(_targetColor, _originalColors[i], t);
                }
            }

            if (t >= 1f)
            {
                t = 0f;
                fadingToTarget = !fadingToTarget;
            }

            yield return null;
        }
    }
}

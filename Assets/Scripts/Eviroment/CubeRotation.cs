using UnityEngine;
using System.Collections;
using System.Linq;
using Unity.VisualScripting;

public class CubeRotation : MonoBehaviour
{
    private Quaternion _targetRotation;
    private float rotationSpeed = 200f;
    public bool _shouldRotate = false;
    public float _rotationTurn = 90f;
    public bool _canRotate = true;
    public float _rotateCooldown = 2f;

    // Blink figura
    private Color[] _originalColors;
    private Coroutine _blinkCoroutine;
    private float _fadeSpeed = 0.6f;
    private Color _targetColor;

    private GameObject _pivotInstance;

    public bool _isInCooldown = false;
    public CameraChange _cameraChange;
    private CubeAnimation _cubeAnimation;
    public bool _didRotate = false;

    public bool isChildCube = false;

    // Outline de la figura
    private MeshRenderer[] _renderers;
    private Material[][] _materials;

    [SerializeField] private string _outlineProperty = "_OutlineScale";
    [SerializeField] private float _outlineOnValue = 1.03f;
    [SerializeField] private float _outlineOffValue = 0f;

    public GameObject _pivotPoint;
    void Awake()
    {
        _cameraChange = FindFirstObjectByType<CameraChange>();

        _renderers = GetComponentsInChildren<MeshRenderer>(true);
        _materials = new Material[_renderers.Length][];

        for (int i = 0; i < _renderers.Length; i++)
        {
            _materials[i] = _renderers[i].materials;

            foreach (Material mat in _materials[i])
            {
                if (mat.HasProperty(_outlineProperty))
                {
                    mat.SetFloat(_outlineProperty, _outlineOffValue);
                }
            }
        }
    }

    void Start()
    {
        //  Filtrar los renderers que NO estén en "Objects"
        /*   int objectsLayer = LayerMask.NameToLayer("Objects");

           _renderers = GetComponentsInChildren<MeshRenderer>(true)
               .Where(r => r.gameObject.layer != objectsLayer)
               .ToArray();

           _originalColors = new Color[_renderers.Length];

           for (int i = 0; i < _renderers.Length; i++)
           {
               _originalColors[i] = _renderers[i].material.color;
           }*/

        _cubeAnimation = FindAnyObjectByType<CubeAnimation>();
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
        if (isChildCube) return;

        if (_canRotate && !_shouldRotate && !_isInCooldown)
        {
            Quaternion newTargetRotation = Quaternion.AngleAxis(_rotationTurn, rotationAxis) * transform.rotation;

            if (Quaternion.Angle(transform.rotation, newTargetRotation) > 0.1f)
            {
                _cubeAnimation.IgnoreStretchAndSquash(1f);
                _targetRotation = newTargetRotation;
                _shouldRotate = true;
                _didRotate = true;
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.soundSource.PlayOneShot(AudioManager.Instance._turning);
                }
              
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
            ShowPivotPoint();
        }
        _canRotate = true;
    }

    public void StopBlinking()
    {
        if (_blinkCoroutine != null)
        {
            StopCoroutine(_blinkCoroutine);
            _blinkCoroutine = null;
            DestroyPivotPoint();
            for (int i = 0; i < _renderers.Length; i++)
            {
                _renderers[i].material.color = _originalColors[i];
            }
            _canRotate = false;
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

    private void ShowPivotPoint()
    {
        _pivotPoint.SetActive(true);
    }

    private void DestroyPivotPoint()
    {
       _pivotPoint.SetActive(false);
    }

    public void StartOutline()
    {
        SetOutline(true);

        ShowPivotPoint();
        _canRotate = true;
    }

    public void StopOutline()
    {
        SetOutline(false);

        DestroyPivotPoint();
        _canRotate = false;
    }

    private void SetOutline(bool enabled)
    {
        float value = enabled ? _outlineOnValue : _outlineOffValue;

        foreach (Material[] materialArray in _materials)
        {
            foreach (Material mat in materialArray)
            {
                if (mat.HasProperty(_outlineProperty))
                {
                    mat.SetFloat(_outlineProperty, value);
                }
            }
        }
    }
}


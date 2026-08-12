using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerTransformation : MonoBehaviour
{
    public int _totalTrans = 3;
    public int _currentTrans = 0;
    public int _transformUpgrade = 1;
    public int _restartTrans;
    public CameraChange _cameraChange;
    public TextMeshProUGUI _textTrans;

    [Header("Feedback visual")]
    public Color _normalColor = Color.white;
    public Color _lastFlipColor = Color.yellow;
    public Color _zeroFlipColor = Color.red;
    public float _pulseScale = 1.3f;
    public float _pulseDuration = 0.2f;

    [Header("Feedback sonoro")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _noFlipsSound;

    private bool _isBlinking = false;
    private bool _isPulsing = false;
    [SerializeField] public Button _cheatButton;
    private bool _cheatOn = false;

    private PlayerControls _inputActions;

    void Awake()
    {
        _inputActions = new PlayerControls();
    }

    void OnEnable()
    {
        _inputActions.Camera.Enable();
        _inputActions.Camera.CameraFlip.performed += OnCameraFlipPressed;
    }

    void OnDisable()
    {
        _inputActions.Camera.CameraFlip.performed -= OnCameraFlipPressed;
        _inputActions.Camera.Disable();
    }

    void Start()
    {
        _restartTrans = _totalTrans;
        UpdateTransText();
    }

    private void OnCameraFlipPressed(InputAction.CallbackContext context)
    {
        if (ChangeScene.IsPaused) return;

        if (_currentTrans < _totalTrans)
        {
            _cameraChange._canChange = true;
        }
        else
        {
            _cameraChange._canChange = false;

            if (!_isBlinking)
            {
                PlayNoFlipsSound();
                StartCoroutine(BlinkText());
            }
        }
    }

    private void PlayNoFlipsSound()
    {
        if (_audioSource != null && _noFlipsSound != null)
        {
            _audioSource.PlayOneShot(_noFlipsSound);
        }
    }

    public void PlayerTransformed()
    {
        _currentTrans++;
        UpdateTransText();
    }

    private void UpdateTransText()
    {
        int remaining = _totalTrans - _currentTrans;
        if (remaining < 0) remaining = 0;

        _textTrans.text = "Flips: " + remaining;

        if (remaining <= 0)
        {
            _textTrans.color = _zeroFlipColor;
        }
        else if (remaining == 1)
        {
            _textTrans.color = _lastFlipColor;
            if (!_isPulsing)
                StartCoroutine(PulseText());
        }
        else
        {
            _textTrans.color = _normalColor;
        }
    }

    IEnumerator BlinkText()
    {
        _isBlinking = true;

        for (int i = 0; i < 4; i++)
        {
            _textTrans.enabled = false;
            yield return new WaitForSeconds(0.15f);
            _textTrans.enabled = true;
            yield return new WaitForSeconds(0.15f);
        }

        _isBlinking = false;
    }

    IEnumerator PulseText()
    {
        _isPulsing = true;
        Vector3 originalScale = _textTrans.transform.localScale;
        Vector3 targetScale = originalScale * _pulseScale;

        float t = 0f;
        while (t < _pulseDuration)
        {
            t += Time.deltaTime;
            _textTrans.transform.localScale = Vector3.Lerp(originalScale, targetScale, t / _pulseDuration);
            yield return null;
        }

        t = 0f;
        while (t < _pulseDuration)
        {
            t += Time.deltaTime;
            _textTrans.transform.localScale = Vector3.Lerp(targetScale, originalScale, t / _pulseDuration);
            yield return null;
        }

        _textTrans.transform.localScale = originalScale;
        _isPulsing = false;
    }

    public void CheatTransformation()
    {
        _cheatOn = !_cheatOn;
        if (_cheatOn)
        {
            _totalTrans = 1000;
        }
        else
        {
            _totalTrans = _restartTrans;
        }
        UpdateTransText();
    }

    public void TransformUpgrade()
    {
        _totalTrans = _totalTrans + _transformUpgrade;
        UpdateTransText();
        StartCoroutine(BlinkText());
    }
}
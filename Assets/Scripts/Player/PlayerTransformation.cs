using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class PlayerTransformation : MonoBehaviour
{
    public int _totalTrans = 3;
    public int _currentTrans = 0;
    public int _restartTrans;
    public CameraChange _cameraChange;
    public TextMeshProUGUI _textTrans;

    private bool _isBlinking = false;
    [SerializeField] Button _cheatButton;
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
        _textTrans.text = "Flips: " + _currentTrans + "/" + _totalTrans;
        _restartTrans = _totalTrans;
    }

    private void OnCameraFlipPressed(InputAction.CallbackContext context)
    {
        if (_currentTrans >= _totalTrans)
        {
            _cameraChange._canChange = false;

            if (!_isBlinking)
                StartCoroutine(BlinkText());
        }
    }

    public void PlayerTransformed()
    {
        _currentTrans++;
        _textTrans.text = "Flips: " + _currentTrans + "/" + _totalTrans;
        Debug.Log("Transformations: " + _currentTrans);
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
        _textTrans.text = "Flips: " + _currentTrans + "/" + _totalTrans;
    }
}
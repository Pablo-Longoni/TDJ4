using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class TutorialDisplay : MonoBehaviour
{
    [SerializeField] GameObject _wasdImage;
    [SerializeField] GameObject _spaceBarImage;
    [SerializeField] GameObject _mouseClickImage;
    [SerializeField] GameObject _mouseMoveImage;
    [SerializeField] GameObject _mouseMiddleImage;

    private bool _didClick = false;
    private bool _didMove = false;
    private bool _didZoom = false;
    private bool _didTrans = false;
    private bool _canDetectMovement = false;

    private Vector2 _mouseDelta;
    private PlayerControls _input;
    private string _scene;

    void Awake()
    {
        _input = new PlayerControls();

        // Guardamos el delta del mouse cada vez que se mueva
        _input.Camera.MouseDelta.performed += ctx => _mouseDelta = ctx.ReadValue<Vector2>();
    }

    void OnEnable()
    {
        _input.Camera.Enable();
        _input.Player.Enable(); // importante para el movimiento WASD
    }

    void OnDisable()
    {
        _input.Camera.Disable();
        _input.Player.Disable();
    }

    void Start()
    {
        _scene = SceneManager.GetActiveScene().name;

        if (_scene == "Level2")
        {
            TransformTutorial();
        }
        else if (_scene == "Level3")
        {
            MouseTutorial();
        }
    }

    void Update()
    {
        if (_scene == "Level2")
        {
            TransformTutorial();
        }
        else if (_scene == "Level3")
        {
            MouseTutorial();
        }
    }

    void TransformTutorial()
    {
        if (_input.Camera.CameraFlip.triggered && !_didTrans)
        {
            _spaceBarImage.SetActive(false);
            _wasdImage.SetActive(true);
            _didTrans = true;
        }

        Vector2 moveInput = _input.Player.Movement.ReadValue<Vector2>();
        if (moveInput.magnitude > 0.1f)
        {
            _wasdImage.SetActive(false);
        }
    }

    void MouseTutorial()
    {
        bool clickPressed = _input.Camera.Click.triggered;

        if (!_didClick && clickPressed)
        {
            _didClick = true;
            _mouseClickImage.SetActive(true);
            _mouseMoveImage.SetActive(true);
            StartCoroutine(EnableMovementDetection());
        }

        if (_didClick && !_didMove && _canDetectMovement && clickPressed)
        {
            if (_mouseDelta.magnitude > 5f)
            {
                _didMove = true;
                _mouseMoveImage.SetActive(false);
                _mouseClickImage.SetActive(false);
                _mouseMiddleImage.SetActive(true);
            }
        }

        if (_didMove && !_didZoom &&
            (_input.Camera.ZoomIn.triggered || _input.Camera.ZoomOut.triggered))
        {
            _didZoom = true;
            _mouseMiddleImage.SetActive(false);
        }
    }

    IEnumerator EnableMovementDetection()
    {
        yield return new WaitForSeconds(1.5f);
        _canDetectMovement = true;
    }
}


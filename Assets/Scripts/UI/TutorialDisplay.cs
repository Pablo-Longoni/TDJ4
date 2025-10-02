using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;

public class TutorialDisplay : MonoBehaviour
{
    [Header("Keyboard / Mouse UI")]
    [SerializeField] GameObject _wasdImage;
    [SerializeField] GameObject _spaceBarImage;
    [SerializeField] GameObject _mouseClickImage;
    [SerializeField] GameObject _mouseMoveImage;
    [SerializeField] GameObject _mouseMiddleImage;

    [Header("Gamepad UI")]
    [SerializeField] GameObject _leftStickImage;
    [SerializeField] GameObject _rightStickImage;
    [SerializeField] GameObject _diagonal;
    [SerializeField] GameObject _southButtonImage;
    [SerializeField] GameObject _northButtonImage;
    [SerializeField] GameObject _westButtonImage;
    [SerializeField] GameObject _zoomL2;
    [SerializeField] GameObject _zoomR2;

    private bool _didClick = false;
    private bool _didMove = false;
    private bool _didZoom = false;
    private bool _didTrans = false;
    private bool _canDetectMovement = false;


    private Vector2 _mouseDelta;
    private PlayerControls _input;
    private string _scene;

    [SerializeField] private Transform cameraTransform;
    private float _lastX;
    private float _accumulatedRotation;
    private bool _didRotate;
    //  public string _currentScheme = "Keyboard"; 
    void Awake()
    {
        _input = new PlayerControls();

        // Guardamos el delta del mouse cada vez que se mueva
        _input.Camera.MouseDelta.performed += ctx => _mouseDelta = ctx.ReadValue<Vector2>();

     //   _input.Player.Movement.performed += DetectControlScheme;
     //   _input.Camera.Click.performed += DetectControlScheme;
     ///   _input.Camera.ZoomIn.performed += DetectControlScheme;
    }

    /*    void DetectControlScheme(InputAction.CallbackContext ctx)
        {
            var device = ctx.control.device;
            if (device is Gamepad)
            {
                if (_currentScheme != "Gamepad")
                {
                    _currentScheme = "Gamepad";
                    SwitchToGamepadUI();
                }
            }
            else if (device is Keyboard || device is Mouse)
            {
                if (_currentScheme != "Keyboard")
                {
                    _currentScheme = "Keyboard";
                    SwitchToKeyboardUI();
                }
            }
        }
        void SwitchToKeyboardUI()
        {
            // Apagar gamepad UI
            _leftStickImage.SetActive(false);
            _southButtonImage.SetActive(false);
            _rightStickImage.SetActive(false);
            _zoom.SetActive(false);

            // Encender teclado/mouse UI inicial
            _wasdImage.SetActive(true);
            _spaceBarImage.SetActive(false);
        }

        void SwitchToGamepadUI()
        {
            // Apagar teclado/mouse UI
            _wasdImage.SetActive(false);
            _spaceBarImage.SetActive(false);
            _mouseClickImage.SetActive(false);
            _mouseMoveImage.SetActive(false);
            _mouseMiddleImage.SetActive(false);

            // Encender gamepad UI inicial
            _leftStickImage.SetActive(true);
            _southButtonImage.SetActive(false);
        }*/
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

        if(_scene == "Level1-Flat")
        {
            MoveTutorial();
        }

        else  if (_scene == "Level2-Rotate")
        {
            _lastX = cameraTransform.eulerAngles.y; 
          //  Debug.Log("La cámara rotó" + _lastX);
            TransformTutorial();
        }
        else if (_scene == "Level3-Fall")
        {
            MouseTutorial();
        }
    }

    void Update()
    {

        if (_scene == "Level1-Flat")
        {
            MoveTutorial();
        }
        else if (_scene == "Level2-Rotate")
        {
            TransformTutorial();
        }
        else if (_scene == "Level3-Fall")
        {
            MouseTutorial();
        }
    }

    void MoveTutorial()
    {
       _wasdImage.SetActive(true);
       _leftStickImage.SetActive(true);
       _diagonal.SetActive(true);
    }

    void TransformTutorial()
    {
       // Debug.Log("TUTORIAL TECLADO");
        if (_input.Camera.CameraFlip.triggered && !_didTrans) 
        { 
            _spaceBarImage.SetActive(false); 
            _southButtonImage.SetActive(false);
            _wasdImage.SetActive(true);
            _leftStickImage.SetActive(true);
            _didTrans = true; 
          //  Debug.Log("afuera space TUTORIAL TECLADO"); 
        }

        Vector2 moveInput = _input.Player.Movement.ReadValue<Vector2>();

        if (moveInput.magnitude > 0.1f && _didTrans) 
        { 
            _wasdImage.SetActive(false);
            _leftStickImage.SetActive(false);
            _diagonal.SetActive(false);
        }

    }

    void MouseTutorial()
    {
        if (!_didRotate)
        {
            float currentX = cameraTransform.eulerAngles.y;
            float delta = Mathf.DeltaAngle(_lastX, currentX); // diferencia real entre frames

            _accumulatedRotation += Mathf.Abs(delta);

            if (_accumulatedRotation > 50f) // si la diferencia supera 10 grados
            {
                _didRotate = true;

                _mouseMoveImage.SetActive(false);
                _mouseClickImage.SetActive(false);
                _rightStickImage.SetActive(false);
                _diagonal.SetActive(false);

                _westButtonImage.SetActive(true);
                _mouseMiddleImage.SetActive(true);
                _zoomL2.SetActive(true);
                _zoomR2.SetActive(true);
               // Debug.Log("La cámara rotó" + delta + currentX);
            }

            _lastX = currentX;
        }
            /*  bool clickPressed = _input.Camera.Click.triggered; 

              if (!_didClick && clickPressed)
              {
                  _didClick = true; _mouseClickImage.SetActive(true);
                  _mouseMoveImage.SetActive(true);
                  StartCoroutine(EnableMovementDetection());
              }

              if (_didClick && !_didMove && _canDetectMovement) 
              {
                  Debug.Log("MOUSE RUEDA");
                  if (_mouseDelta.magnitude > 2f)
                  { 
                      _didMove = true; _mouseMoveImage.SetActive(false);
                      _mouseClickImage.SetActive(false);
                      _mouseMiddleImage.SetActive(true); 
                  }
              }

              if (_didMove && !_didZoom && (_input.Camera.ZoomIn.triggered || _input.Camera.ZoomOut.triggered))
              {
                  _didZoom = true; _mouseMiddleImage.SetActive(false);
              }*/
        }

    IEnumerator EnableMovementDetection()
    {
        yield return new WaitForSeconds(1.5f);
        _canDetectMovement = true;
    }

}


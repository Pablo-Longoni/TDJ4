using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement; // para reiniciar la escena

public class PlayerInputReader : MonoBehaviour
{
    private PlayerControls _controls;

    public Vector2 MoveInput { get; private set; }
    public bool GrabPressed { get; private set; }

    public Vector2 LookInput { get; private set; }
    public bool RotateCameraPressed { get; private set; }
    public Vector2 MouseDelta { get; private set; }
    public bool CameraHelpPressed { get; private set; }
    public bool CameraFlipTriggered { get; private set; }

    public bool ZoomInHeld { get; private set; }
    public bool ZoomOutHeld { get; private set; }
    public bool MenuTogglePressed { get; private set; }
    public Vector2 NavigateInput { get; private set; }

    public bool RestartKeyPressed { get; private set; }

    // --- Restart con botón ---
    private float _restartHoldTime = 3f;  // segundos necesarios
    private float _buttonBPressStart = -1f; // tiempo de inicio

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += _ => MoveInput = Vector2.zero;

        _controls.Player.Grab.performed += _ => GrabPressed = true;
        _controls.Player.Grab.canceled += _ => GrabPressed = false;

        _controls.Camera.RotateCamera.performed += _ => RotateCameraPressed = true;
        _controls.Camera.RotateCamera.canceled += _ => RotateCameraPressed = false;

        _controls.Camera.MouseDelta.performed += ctx => MouseDelta = ctx.ReadValue<Vector2>();
        _controls.Camera.MouseDelta.canceled += _ => MouseDelta = Vector2.zero;

        _controls.Camera.CameraHelp.performed += _ => CameraHelpPressed = true;
        _controls.Camera.CameraHelp.canceled += _ => CameraHelpPressed = false;

        _controls.Camera.CameraFlip.performed += _ => CameraFlipTriggered = true;

        _controls.Camera.ZoomIn.performed += _ => ZoomInHeld = true;
        _controls.Camera.ZoomIn.canceled += _ => ZoomInHeld = false;

        _controls.Camera.ZoomOut.performed += _ => ZoomOutHeld = true;
        _controls.Camera.ZoomOut.canceled += _ => ZoomOutHeld = false;

        _controls.UI.MenuOpenClose.performed += _ => MenuTogglePressed = true;
        _controls.UI.MenuOpenClose.canceled += _ => MenuTogglePressed = false;

        _controls.UI.Navigate.performed += ctx => NavigateInput = ctx.ReadValue<Vector2>();
        _controls.UI.Navigate.canceled += _ => NavigateInput = Vector2.zero;

        _controls.Camera.Look.performed += ctx => LookInput = ctx.ReadValue<Vector2>();
        _controls.Camera.Look.canceled += _ => LookInput = Vector2.zero;
    }

    private void OnEnable()
    {
        _controls.Enable();
        _controls.Gameplay.Enable();
        _controls.Camera.Enable();
        _controls.UI.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    private void Update()
    {
        if (Gamepad.current != null)
        {
            // Si se empieza a apretar el botón B
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                _buttonBPressStart = Time.time;
            }

            // Si se suelta antes de tiempo, reseteamos
            if (Gamepad.current.buttonEast.wasReleasedThisFrame)
            {
                _buttonBPressStart = -1f;
            }

            // Si se mantiene presionado lo suficiente
            if (_buttonBPressStart > 0f && Gamepad.current.buttonEast.isPressed)
            {
                if (Time.time - _buttonBPressStart >= _restartHoldTime)
                {
                    RestartLevel();
                    _buttonBPressStart = -1f; // para que no reinicie en loop
                }
            }
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetFlags()
    {
        CameraFlipTriggered = false;
        CameraHelpPressed = false;
        MenuTogglePressed = false;
    }
}

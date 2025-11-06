using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerControls _controls;

    public Vector2 MoveInput { get; private set; }
    public bool GrabPressed { get; private set; }

    public Vector2 LookInput { get; private set; }
    public bool RotateCameraPressed { get; private set; }
    //public Vector2 MouseDelta { get; private set; }
    public bool CameraHelpPressed { get; private set; }
    public bool CameraFlipTriggered { get; private set; }

    public bool ZoomInHeld { get; private set; }
    public bool ZoomOutHeld { get; private set; }
    public bool MenuTogglePressed { get; private set; }
    public Vector2 NavigateInput { get; private set; }

    public bool RestartKeyPressed { get; private set; }
    public bool IsPinching
    {
        get
        {
            if (ChangeScene.IsPaused) return false;
            return Touchscreen.current != null &&
                   Touchscreen.current.touches.Count >= 2 &&
                   Touchscreen.current.touches[0].isInProgress &&
                   Touchscreen.current.touches[1].isInProgress;
        }
    }

    public Vector2 Touch0Position => Touchscreen.current?.touches[0].position.ReadValue() ?? Vector2.zero;
    public Vector2 Touch1Position => Touchscreen.current?.touches[1].position.ReadValue() ?? Vector2.zero;

    private float _restartHoldTime = 3f;
    private float _buttonBPressStart = -1f;

    private void Awake()
    {
        _controls = new PlayerControls();

        // MODIFICADO: Agregar verificación de pausa en inputs de gameplay
        _controls.Gameplay.Move.performed += ctx =>
        {
            if (ChangeScene.IsPaused) return;
            MoveInput = ctx.ReadValue<Vector2>();
        };
        _controls.Gameplay.Move.canceled += _ => MoveInput = Vector2.zero;

        _controls.Player.Grab.performed += _ =>
        {
            if (ChangeScene.IsPaused) return;
            GrabPressed = true;
        };
        _controls.Player.Grab.canceled += _ => GrabPressed = false;

        _controls.Camera.RotateCamera.performed += _ =>
        {
            if (ChangeScene.IsPaused) return;
            RotateCameraPressed = true;
        };
        _controls.Camera.RotateCamera.canceled += _ => RotateCameraPressed = false;

        /*_controls.Camera.MouseDelta.performed += ctx =>
        {
            if (ChangeScene.IsPaused) return;
            MouseDelta = ctx.ReadValue<Vector2>();
        };
        _controls.Camera.MouseDelta.canceled += _ => MouseDelta = Vector2.zero;
*/
        _controls.Camera.CameraHelp.performed += _ =>
        {
            if (ChangeScene.IsPaused) return;
            CameraHelpPressed = true;
        };
        _controls.Camera.CameraHelp.canceled += _ => CameraHelpPressed = false;

        _controls.Camera.CameraFlip.performed += _ =>
        {
            if (ChangeScene.IsPaused) return;
            CameraFlipTriggered = true;
        };

        _controls.Camera.ZoomIn.performed += _ =>
        {
            if (ChangeScene.IsPaused) return;
            ZoomInHeld = true;
        };
        _controls.Camera.ZoomIn.canceled += _ => ZoomInHeld = false;

        _controls.Camera.ZoomOut.performed += _ =>
        {
            if (ChangeScene.IsPaused) return;
            ZoomOutHeld = true;
        };
        _controls.Camera.ZoomOut.canceled += _ => ZoomOutHeld = false;

        // UI inputs NO necesitan verificación de pausa (queremos que funcionen en pausa)
        _controls.UI.MenuOpenClose.performed += _ => MenuTogglePressed = true;
        _controls.UI.MenuOpenClose.canceled += _ => MenuTogglePressed = false;

        _controls.UI.Navigate.performed += ctx => NavigateInput = ctx.ReadValue<Vector2>();
        _controls.UI.Navigate.canceled += _ => NavigateInput = Vector2.zero;

        _controls.Camera.Look.performed += ctx =>
        {
            if (ChangeScene.IsPaused) return;
            LookInput = ctx.ReadValue<Vector2>();
        };
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
        // MODIFICADO: No permitir restart durante pausa
        if (ChangeScene.IsPaused) return;

        if (Gamepad.current != null)
        {
            if (Gamepad.current.buttonEast.wasPressedThisFrame)
            {
                _buttonBPressStart = Time.time;
            }

            if (Gamepad.current.buttonEast.wasReleasedThisFrame)
            {
                _buttonBPressStart = -1f;
            }

            if (_buttonBPressStart > 0f && Gamepad.current.buttonEast.isPressed)
            {
                if (Time.time - _buttonBPressStart >= _restartHoldTime)
                {
                    RestartLevel();
                    _buttonBPressStart = -1f;
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
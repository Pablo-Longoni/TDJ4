using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    private PlayerControls _controls;

    public Vector2 MoveInput { get; private set; }
    public bool GrabPressed { get; private set; }

    public bool RotateLeftHeld { get; private set; }
    public bool RotateRightHeld { get; private set; }
    public Vector2 MouseDelta { get; private set; }
    public bool CameraHelpPressed { get; private set; }
    public bool CameraFlipTriggered { get; private set; }

    public bool ZoomInHeld { get; private set; }
    public bool ZoomOutHeld { get; private set; }
    public bool RestartKeyPressed => Keyboard.current != null && Keyboard.current.rKey.isPressed;

    public bool MenuTogglePressed { get; private set; }
    public Vector2 NavigateInput { get; private set; }

    private void Awake()
    {
        _controls = new PlayerControls();

        _controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += _ => MoveInput = Vector2.zero;

        _controls.Player.Grab.performed += _ => GrabPressed = true;
        _controls.Player.Grab.canceled += _ => GrabPressed = false;

        _controls.Camera.RotateLeft.performed += _ => RotateLeftHeld = true;
        _controls.Camera.RotateLeft.canceled += _ => RotateLeftHeld = false;

        _controls.Camera.RotateRight.performed += _ => RotateRightHeld = true;
        _controls.Camera.RotateRight.canceled += _ => RotateRightHeld = false;

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
    }

    private void OnEnable()
    {
        _controls.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }

    public void ResetFlags()
    {
        CameraFlipTriggered = false;
        CameraHelpPressed = false;
        MenuTogglePressed = false;
    }
}

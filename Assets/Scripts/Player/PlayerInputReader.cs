using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour
{
    public Vector2 MoveInput { get; private set; }
    public float RotateInput { get; private set; }

    private PlayerControls _controls;

    private void Awake()
    {
        _controls = new PlayerControls();
    }

    private void OnEnable()
    {
        _controls.Enable();

        
        _controls.Gameplay.Move.performed += ctx => MoveInput = ctx.ReadValue<Vector2>();
        _controls.Gameplay.Move.canceled += ctx => MoveInput = Vector2.zero;

       
        //_controls.UI.RotateCamera.performed += ctx => RotateInput = ctx.ReadValue<float>();
        //_controls.UI.RotateCamera.canceled += ctx => RotateInput = 0f;

        _controls.UI.Enable();
    }

    private void OnDisable()
    {
        _controls.Disable();
    }
}


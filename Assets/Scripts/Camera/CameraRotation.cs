using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRotation : MonoBehaviour
{
    public float _rotationSpeed = 1000f;
    public CameraChange _cameraChange;

    private PlayerInputReader _input;

    private void Awake()
    {
        _input = FindObjectOfType<PlayerInputReader>();
    }

    void Update()
    {
        if (_input == null || !_cameraChange._isIsometric) return;

        
        if (Mouse.current.leftButton.isPressed)
        {
            Vector2 mouseDelta = _input.MouseDelta;
            transform.Rotate(Vector3.up, mouseDelta.x * _rotationSpeed * Time.deltaTime);
        }

        
        if (_input.RotateLeftHeld)
        {
            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
        }

        if (_input.RotateRightHeld)
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}

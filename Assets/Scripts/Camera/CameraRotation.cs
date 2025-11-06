using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float _rotationSpeed = 1000f;
    public CameraChange _cameraChange;
    public PlayerInputReader inputReader;

    [Header("Joystick Reference")]
    public DynamicJoystick cameraJoystick; // ✅ Solo el joystick de la cámara

    private void Start()
    {
        _rotationSpeed = PlayerPrefs.GetFloat("RotationSpeed", 360);
        Debug.Log("Rotation: " + _rotationSpeed);
    }

    void Update()
    {
        if (!_cameraChange._isIsometric) return;

        // ✅ Rotar SOLO con el joystick de cámara
        if (cameraJoystick != null)
        {
            Vector2 joystickInput = new Vector2(cameraJoystick.Horizontal, cameraJoystick.Vertical);

            if (Mathf.Abs(joystickInput.x) > 0.1f)
            {
                transform.Rotate(Vector3.up, joystickInput.x * _rotationSpeed * Time.deltaTime);
            }
        }

        // --- ROTACIÓN CON BOTONES ---
        if (Input.GetButton("RotateLeft"))
        {
            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButton("RotateRight"))
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }

        // --- ROTACIÓN CON GAMEPAD ---
        Vector2 look = inputReader.LookInput;
        if (Mathf.Abs(look.x) > 0.1f)
        {
            transform.Rotate(Vector3.up, look.x * _rotationSpeed * Time.deltaTime);
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void SetRotationSpeed(float newSpeed)
    {
        _rotationSpeed = newSpeed;
    }
}
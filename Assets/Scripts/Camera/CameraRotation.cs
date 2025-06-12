using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float _rotationSpeed = 1000f;
    public CameraChange _cameraChange;

    void Update()
    {
        if (!_cameraChange._isIsometric) return;

        // Rotación con mouse (manteniendo clic izquierdo)
        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * _rotationSpeed * Time.deltaTime);
        }

        // Rotación con Gamepad (L1 / R1)
        if (Input.GetButton("RotateLeft"))
        {
            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButton("RotateRight"))
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }
    }

    public void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
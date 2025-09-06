using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float _rotationSpeed = 1000f;
    public CameraChange _cameraChange;
    public PlayerInputReader inputReader;

    void Update()
    {
        if (!_cameraChange._isIsometric) return;


        if (Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * _rotationSpeed * Time.deltaTime);
        }


        if (Input.GetButton("RotateLeft"))
        {
            transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
        }

        if (Input.GetButton("RotateRight"))
        {
            transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime);
        }


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
}
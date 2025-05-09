using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float _rotationSpeed = 1000f;
    public CameraChange _cameraChange;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && _cameraChange._isIsometric) 
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * _rotationSpeed * Time.deltaTime);
        }
    }

   public void ResetRotation()
    {
        Vector3 angles = transform.eulerAngles;
        angles.z = 0;
        angles.y = 0;
        angles.x = 0;
        transform.rotation = Quaternion.Euler(angles);
    }
}

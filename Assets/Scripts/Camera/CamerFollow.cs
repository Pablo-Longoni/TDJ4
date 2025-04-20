using UnityEngine;

public class CamerFollow : MonoBehaviour
{
    public Transform targetCamera;

    void LateUpdate()
    {
        if (targetCamera != null)
        {
            transform.position = targetCamera.position;
            transform.rotation = targetCamera.rotation;
        }
    }
}

using UnityEngine;

public class CubeFace : MonoBehaviour
{
    public Vector3 GetGravityDirection()
    {
        return -transform.up; // 'Abajo' de la cara es -up
    }

    public Vector3 GetForward()
    {
        return transform.forward;
    }

    public Vector3 GetRight()
    {
        return transform.right;
    }
}

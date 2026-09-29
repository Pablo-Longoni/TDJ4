using UnityEngine;

public class PlayerGroundDetection : MonoBehaviour
{
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private CubeAnimation cubeAnimation;

    public bool IsGrounded { get; private set; }

    private int groundContacts = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!IsGroundLayer(other.gameObject))
            return;

        groundContacts++;
        IsGrounded = true;

        cubeAnimation.OnGroundDetected();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!IsGroundLayer(other.gameObject))
            return;

        groundContacts--;

        if (groundContacts <= 0)
        {
            groundContacts = 0;
            IsGrounded = false;
        }
    }

    private bool IsGroundLayer(GameObject obj)
    {
        return ((1 << obj.layer) & groundLayer) != 0;
    }
}
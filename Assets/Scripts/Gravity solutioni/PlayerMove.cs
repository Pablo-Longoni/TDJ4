using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 5f;
    public GravityPlayer gravityScript;

    void Update()
    {
        if (gravityScript.currentFace == null) return;

        // Movimiento en el plano de la cara
        Vector3 forward = gravityScript.currentFace.GetForward();
        Vector3 right = gravityScript.currentFace.GetRight();

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 moveDir = (right * h + forward * v).normalized;
        transform.position += moveDir * speed * Time.deltaTime;
    }
}


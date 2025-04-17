using UnityEngine;

public class GravityPlayer : MonoBehaviour
{
    public Transform gravityTarget; // El objeto central, como el cubo o planeta
    public float gravityForce = 9.8f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Desactivar la gravedad global
        rb.constraints = RigidbodyConstraints.FreezeRotation; // Opcional: evita que se gire con colisiones
    }

    void FixedUpdate()
    {
        Vector3 gravityDirection = (gravityTarget.position - transform.position).normalized;
        rb.AddForce(gravityDirection * gravityForce, ForceMode.Acceleration);

        // Rotar al jugador para que "pegue los pies" al cubo
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, gravityDirection) * transform.rotation;
        rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.fixedDeltaTime));
    }
}

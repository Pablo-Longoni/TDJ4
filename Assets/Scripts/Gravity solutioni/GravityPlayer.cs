using UnityEngine;

public class GravityPlayer : MonoBehaviour
{
    public float gravityForce = 9.8f;
    private Rigidbody rb;
    public CubeFace currentFace;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        AlignToFace(currentFace);
    }

    void FixedUpdate()
    {
        if (currentFace != null)
        {
            Vector3 gravityDir = currentFace.GetGravityDirection();
            rb.AddForce(gravityDir * gravityForce, ForceMode.Acceleration);
        }
    }

    public void AlignToFace(CubeFace face)
    {
        currentFace = face;
        Vector3 gravityDir = face.GetGravityDirection();

        // Alineamos la rotación del jugador para que su 'abajo' coincida con la cara
        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, -gravityDir) * transform.rotation;
        transform.rotation = targetRotation;

        // Opcional: reubicar al centro de la cara o al punto más cercano
        Vector3 facePosition = face.transform.position;
        transform.position = facePosition + face.transform.up * 0.5f; // Despegado un poco
    }
}

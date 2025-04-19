using UnityEngine;

public class FaceNode : MonoBehaviour
{
    public CubeRotation parentCube;
    public FaceNode forward;
    public FaceNode back;
    public FaceNode left;
    public FaceNode right;

    // El eje alrededor del cual rotaría el cubo para llegar a esta cara desde la anterior
    public Vector3 rotationAxis;

    // Si necesitás, podés guardar un tipo o índice para depurar
}

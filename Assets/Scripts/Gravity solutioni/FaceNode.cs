using UnityEngine;

public class FaceNode : MonoBehaviour
{
    public CubeRotation parentCube;
    public FaceNode forward;
    public FaceNode back;
    public FaceNode left;
    public FaceNode right;

    // El eje alrededor del cual rotar�a el cubo para llegar a esta cara desde la anterior
    public Vector3 rotationAxis;

    // Si necesit�s, pod�s guardar un tipo o �ndice para depurar
}

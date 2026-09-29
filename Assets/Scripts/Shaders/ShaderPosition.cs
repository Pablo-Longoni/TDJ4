using UnityEngine;

[ExecuteInEditMode]
public class ShaderPosition : MonoBehaviour
{
    public float radius = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        Shader.SetGlobalVector("_Position", transform.position);
        Shader.SetGlobalFloat("_Radius", radius);
    }
}

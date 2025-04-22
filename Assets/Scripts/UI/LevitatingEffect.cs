using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevitatingEffect : MonoBehaviour
{
    public float amplitude = 10f;
    public float frequency = 1f;

    private TextMeshProUGUI textMesh;
    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        // Guardamos una copia de los vértices originales
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            originalVertices[i] = textInfo.meshInfo[i].vertices.Clone() as Vector3[];
        }
    }

    void Update()
    {
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            int materialIndex = charInfo.materialReferenceIndex;
            int vertexIndex = charInfo.vertexIndex;

            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
            Vector3[] sourceVertices = originalVertices[materialIndex];

            float offsetY = Mathf.Sin(Time.time * frequency + i * 0.5f) * amplitude;

            for (int j = 0; j < 4; j++)
            {
                destinationVertices[vertexIndex + j] = sourceVertices[vertexIndex + j] + new Vector3(0, offsetY, 0);
            }
        }

        // Aplicar los cambios de malla
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}

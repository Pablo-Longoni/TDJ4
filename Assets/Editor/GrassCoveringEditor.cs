using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrassCovering))]
public class GrassCoveringEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Dibuja todas las variables normales del componente
        DrawDefaultInspector();

        // Obtiene el GrassCovering que estamos editando
        GrassCovering grassCovering = (GrassCovering)target;

        EditorGUILayout.Space();

        // Botón Generate
        if (GUILayout.Button("Generate Grass"))
        {
            grassCovering.GenerateGrass();
        }

        // Botón Clear
        if (GUILayout.Button("Clear Grass"))
        {
            grassCovering.ClearGrass();
        }
    }
}
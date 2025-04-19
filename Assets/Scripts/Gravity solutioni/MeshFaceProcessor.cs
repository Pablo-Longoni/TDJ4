using System.Collections.Generic;
using UnityEngine;


public class MeshFaceProcessor : MonoBehaviour
{
  /* public List<FaceInfo> allFaces = new List<FaceInfo>();

    void Start()
    {
        ProcessMesh();
    }

    void ProcessMesh()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 v0 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 v1 = transform.TransformPoint(vertices[triangles[i + 1]]);
            Vector3 v2 = transform.TransformPoint(vertices[triangles[i + 2]]);

            Vector3 normal = Vector3.Cross(v1 - v0, v2 - v0).normalized;
            Vector3 center = (v0 + v1 + v2) / 3f;

            FaceInfo face = new FaceInfo
            {
                vertices = new Vector3[] { v0, v1, v2 },
                normal = normal,
                center = center
            };

            allFaces.Add(face);
        }

        FindAdjacentFaces();
    }

    void FindAdjacentFaces()
    {
        for (int i = 0; i < allFaces.Count; i++)
        {
            for (int j = i + 1; j < allFaces.Count; j++)
            {
                if (ShareEdge(allFaces[i], allFaces[j]))
                {
                    allFaces[i].adjacentFaces.Add(allFaces[j]);
                    allFaces[j].adjacentFaces.Add(allFaces[i]);
                }
            }
        }
    }

    bool ShareEdge(FaceInfo a, FaceInfo b)
    {
        int sharedVerts = 0;
        foreach (var va in a.vertices)
        {
            foreach (var vb in b.vertices)
            {
                if (Vector3.Distance(va, vb) < 0.01f)
                {
                    sharedVerts++;
                }
            }
        }
        return sharedVerts >= 2;
    }*/
}
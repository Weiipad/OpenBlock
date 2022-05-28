using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestVertexColor : MonoBehaviour
{
    MeshFilter meshFilter;
    public void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        SetColors();
    }

    public void SetColors()
    {
        Mesh mesh = new Mesh();
        mesh.subMeshCount = 2;
        mesh.name = "Gen Mesh";

        mesh.vertices = new Vector3[] { Vector3.zero, Vector3.right, Vector3.up, Vector3.right + Vector3.up };
        mesh.colors = new Color[] { Color.red, Color.red, Color.red, Color.black };

        mesh.SetTriangles(new int[] { 2, 1, 0 }, 0);
        mesh.SetTriangles(new int[] { 3, 1, 2 }, 1);

        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
  
        //mesh.SetSubMesh(0, new UnityEngine.Rendering.SubMeshDescriptor(0, 3));
        //mesh.SetSubMesh(1, new UnityEngine.Rendering.SubMeshDescriptor(3, 3));

        meshFilter.mesh = mesh;
    }
}

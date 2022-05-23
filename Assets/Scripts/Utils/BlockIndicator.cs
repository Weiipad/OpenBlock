using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock
{
    public static class BlockIndicator
    {
        public static Mesh mesh;
        public static Material material;

        public static void Init()
        {
            float e = 0.001f;
            Vector3 pxpypz = Vector3.one * e;
            Vector3 pxpynz = (Vector3.right + Vector3.up + Vector3.back) * e;
            Vector3 pxnypz = (Vector3.right + Vector3.down + Vector3.forward) * e;
            Vector3 nxpypz = (Vector3.left + Vector3.up + Vector3.forward) * e;
            Vector3[] vertices = new Vector3[] 
            {
                Vector3.zero - pxpypz,
                Vector3.right - nxpypz,
                Vector3.right + Vector3.forward + pxnypz,
                Vector3.forward - pxpynz,
                Vector3.up - pxnypz,
                Vector3.up + Vector3.right + pxpynz,
                Vector3.up + Vector3.right + Vector3.forward + pxpypz,
                Vector3.up + Vector3.forward + nxpypz,
            };

            int[] indices = new int[]
            {
                0, 1,  1, 2,  2, 3,
                3, 0,  4, 5,  5, 6,
                6, 7,  7, 4,  0, 4,
                1, 5,  2, 6,  3, 7
            };

            mesh = new Mesh();
            mesh.vertices = vertices;
            mesh.SetIndices(indices, MeshTopology.Lines, 0);

            material = new Material(Shader.Find("Sprites/Default"));
            material.color = Color.black;
        }

        public static void Draw(Vector3 position, int layer, Camera camera)
        {
            if (mesh == null || material == null) Init();
            Graphics.DrawMesh(mesh, position, Quaternion.identity, material, layer, camera);
        }
    }
}

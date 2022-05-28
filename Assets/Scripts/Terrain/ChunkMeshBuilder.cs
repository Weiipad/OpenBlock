using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class ChunkMeshBuilder
    {
        public List<Vector3> vertices;
        public List<Color> colors;

        public int VertexCount => vertices.Count;

        private List<int> commonIndices, colorIndices, tranparentIndices;
        private bool IsOpaqueBlockExist => commonIndices != null && commonIndices.Count > 0;
        private bool IsColorBlockExist => colorIndices != null && colorIndices.Count > 0;
        private bool IsTransparentBlockExist => tranparentIndices != null && tranparentIndices.Count > 0;

        public ChunkMeshBuilder()
        {
            vertices = new List<Vector3>();
            colors = new List<Color>();
        }

        public void AddVertex(Vector3 vertex)
        {
            AddVertex(vertex, Color.black);
        }

        public void AddVertex(Vector3 vertex, Color color)
        {
            vertices.Add(vertex);
            colors.Add(color);
        }

        public void AddIndices(BlockRenderType type, params int[] indices)
        {
            switch (type)
            {
                case BlockRenderType.Opaque:
                    if (commonIndices == null) commonIndices = new List<int>();
                    commonIndices.AddRange(indices);
                    break;

                case BlockRenderType.Colored:
                    if (colorIndices == null) colorIndices = new List<int>();
                    colorIndices.AddRange(indices);
                    break;

                case BlockRenderType.Transparent:
                    if (tranparentIndices == null) tranparentIndices = new List<int>();
                    tranparentIndices.AddRange(indices);
                    break;
            }
        }

        public void Clear()
        {
            vertices.Clear();
            colors.Clear();

            commonIndices = null;
            colorIndices = null;
            tranparentIndices = null;
        }

        public void Build(ref Mesh mesh, out int opaqueIdx, out int colorIdx, out int tranparentIdx)
        {
            mesh.Clear();

            int subMeshCount = 0;
            if (IsOpaqueBlockExist) subMeshCount += 1;
            if (IsColorBlockExist) subMeshCount += 1;
            if (IsTransparentBlockExist) subMeshCount += 1;

            mesh.vertices = vertices.ToArray();
            mesh.colors = colors.ToArray();

            mesh.subMeshCount = subMeshCount;
            int subMeshIndex = 0;
            if (IsOpaqueBlockExist)
            {
                opaqueIdx = subMeshIndex;
                mesh.SetTriangles(commonIndices, subMeshIndex++);
            }
            else
            {
                opaqueIdx = -1;
            }

            if (IsColorBlockExist)
            {
                colorIdx = subMeshIndex;
                mesh.SetTriangles(colorIndices, subMeshIndex++);
            }
            else
            {
                colorIdx = -1;
            }

            if (IsTransparentBlockExist)
            {
                tranparentIdx = subMeshIndex;
                mesh.SetTriangles(tranparentIndices, subMeshIndex++);
            }
            else
            {
                tranparentIdx = -1;
            }

            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.Blocks
{
    public static class BlockBuildHelper
    {
        public static void BuildRGBPlane(ref ChunkMeshBuilder builder, Color color, Vector3 o, Vector3 a, Vector3 b)
        {
            int startVertex = builder.VertexCount;

            builder.AddVertex(o, color);
            builder.AddVertex(o + a, color);
            builder.AddVertex(o + b, color);
            builder.AddVertex(o + a + b, color);

            builder.AddIndices(BlockRenderType.Colored, startVertex + 0, startVertex + 3, startVertex + 1, startVertex + 0, startVertex + 2, startVertex + 3);
        }
    }
}

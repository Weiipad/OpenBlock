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

        public static uint ToColorCode(this Color color)
        {
            byte r = (byte)(color.r * 255.0f);
            byte g = (byte)(color.g * 255.0f);
            byte b = (byte)(color.b * 255.0f);

            uint ans = 0;
            ans |= r;
            ans |= (uint)(g << 8);
            ans |= (uint)(b << 16);
            ans |= (0xf << 24);
            return ans;
        }

        public static Color ToColor(this uint colorCode)
        {
            return new Color((byte)(colorCode >> 0) / 255.0f, (byte)(colorCode >> 8) / 255.0f, (byte)(colorCode >> 16) / 255.0f);
        }
    }
}

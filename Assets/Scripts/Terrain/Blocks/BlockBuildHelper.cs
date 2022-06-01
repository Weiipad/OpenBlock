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
        /*
         * o+b-------------o+a+b
         *  |               /|
         *  |            /   | 
         *  |         /      |
         *  |      /         |
         *  |   /            |
         *  |/               |
         *  o---------------o+a
         */


        public static void BuildRGBPlane(ref ChunkMeshBuilder builder, Color color, Vector3 o, Vector3 a, Vector3 b)
        {
            int startVertex = builder.VertexCount;

            builder.AddVertex(o, color);
            builder.AddVertex(o + a, color);
            builder.AddVertex(o + b, color);
            builder.AddVertex(o + a + b, color);

            builder.AddIndices(BlockRenderType.Colored, startVertex + 0, startVertex + 3, startVertex + 1, startVertex + 0, startVertex + 2, startVertex + 3);
        }

        public static void BuildColoredPlane(ref ChunkMeshBuilder builder, int uvIndex, Color color, Vector3 o, Vector3 a, Vector3 b)
        {
            const int TEXTURE_SIZE = 16;

            float unit = 1.0f / TEXTURE_SIZE;

            int col = uvIndex % TEXTURE_SIZE;
            int row = uvIndex / TEXTURE_SIZE;

            float u = col / (float)TEXTURE_SIZE;
            float v = 1.0f - row / (float)TEXTURE_SIZE;

            int startVertex = builder.VertexCount;

            builder.AddVertex(o, color, new Vector2(u, v - unit));
            builder.AddVertex(o + a, color, new Vector2(u + unit, v - unit));
            builder.AddVertex(o + b, color, new Vector2(u, v));
            builder.AddVertex(o + a + b, color, new Vector2(u + unit, v));

            builder.AddIndices(BlockRenderType.Colored, startVertex + 0, startVertex + 3, startVertex + 1, startVertex + 0, startVertex + 2, startVertex + 3);
        }

        public static void BuildPlane(ref ChunkMeshBuilder builder, int uvIndex, Vector3 o, Vector3 a, Vector3 b)
        {
            const int TEXTURE_SIZE = 16;

            float unit = 1.0f / TEXTURE_SIZE;

            int col = uvIndex % TEXTURE_SIZE;
            int row = uvIndex / TEXTURE_SIZE;
            
            float u = col / (float)TEXTURE_SIZE;
            float v = 1.0f - row / (float)TEXTURE_SIZE;
            
            int startVertex = builder.VertexCount;

            builder.AddVertex(o, new Vector2(u, v - unit));
            builder.AddVertex(o + a, new Vector2(u + unit, v - unit));
            builder.AddVertex(o + b, new Vector2(u, v));
            builder.AddVertex(o + a + b, new Vector2(u + unit, v));

            builder.AddIndices(BlockRenderType.Opaque, startVertex + 0, startVertex + 3, startVertex + 1, startVertex + 0, startVertex + 2, startVertex + 3);
        }
    }
}

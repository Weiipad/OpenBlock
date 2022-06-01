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
        public const int UV_RIGHT = 0;
        public const int UV_UP = 1;
        public const int UV_FORWARD = 2;
        public const int UV_LEFT = 3;
        public const int UV_DOWN = 4;
        public const int UV_BACK = 5;

        public enum UVDir: byte
        {
            Up = 0,
            Right = 1,
            Down = 2,
            Left = 3,
        }

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

            var leftBottomUV = new Vector2(u, v - unit);
            var leftTopUV = new Vector2(u, v);
            var rightBottomUV = new Vector2(u + unit, v - unit);
            var rightTopUV = new Vector2(u + unit, v);

            int startVertex = builder.VertexCount;

            builder.AddVertex(o, color, leftBottomUV);
            builder.AddVertex(o + a, color, rightBottomUV);
            builder.AddVertex(o + b, color, leftTopUV);
            builder.AddVertex(o + a + b, color, rightTopUV);

            builder.AddIndices(BlockRenderType.Colored, startVertex + 0, startVertex + 3, startVertex + 1, startVertex + 0, startVertex + 2, startVertex + 3);
        }

        public static void BuildPlane(ref ChunkMeshBuilder builder, int uvIndex, Vector3 o, Vector3 a, Vector3 b)
        {
            BuildPlane(ref builder, uvIndex, 0, o, a, b);
        }

        public static void BuildPlane(ref ChunkMeshBuilder builder, int uvIndex, UVDir uvDir, Vector3 o, Vector3 a, Vector3 b)
        {
            const int TEXTURE_SIZE = 16;

            float unit = 1.0f / TEXTURE_SIZE;

            int col = uvIndex % TEXTURE_SIZE;
            int row = uvIndex / TEXTURE_SIZE;
            
            float u = col / (float)TEXTURE_SIZE;
            float v = 1.0f - row / (float)TEXTURE_SIZE;

            var texCoords = new Vector2[4];

            texCoords[0] = new Vector2(u, v - unit);
            texCoords[1] = new Vector2(u + unit, v - unit);
            texCoords[2] = new Vector2(u + unit, v);
            texCoords[3] = new Vector2(u, v);

            var offset = (byte)uvDir;

            int startVertex = builder.VertexCount;

            builder.AddVertex(o, texCoords[offset % 4]);
            builder.AddVertex(o + a, texCoords[(1 + offset) % 4]);
            builder.AddVertex(o + b, texCoords[(3 + offset) % 4]);
            builder.AddVertex(o + a + b, texCoords[(2 + offset) % 4]);

            builder.AddIndices(BlockRenderType.Opaque, startVertex + 0, startVertex + 3, startVertex + 1, startVertex + 0, startVertex + 2, startVertex + 3);
        }
    }
}

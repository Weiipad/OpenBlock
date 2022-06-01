using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.Blocks
{
    public class CommonBlock : IBlock
    {
        public const int UV_RIGHT = 0;
        public const int UV_UP = 1;
        public const int UV_FORWARD = 2;
        public const int UV_LEFT = 3;
        public const int UV_DOWN = 4;
        public const int UV_BACK = 5;
        private int[] uvIndices = new int[6];
        public CommonBlock(int uvIndex): this(uvIndex, uvIndex, uvIndex) { }

        public CommonBlock(int axisUV, int sideUV): this(axisUV, sideUV, axisUV) { }

        public CommonBlock(int upUV, int sideUV, int downUV) : this(sideUV, upUV, sideUV, sideUV, downUV, sideUV) { }

        public CommonBlock(int rightUV, int upUV, int forwardUV, int leftUV, int downUV, int backUV)
        {
            uvIndices[UV_RIGHT] = rightUV;
            uvIndices[UV_UP] = upUV;
            uvIndices[UV_FORWARD] = forwardUV;
            uvIndices[UV_LEFT] = leftUV;
            uvIndices[UV_DOWN] = downUV;
            uvIndices[UV_BACK] = backUV;
        }

        public void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing)
        {
            if (facing == BlockFacing.None) return;

            int south = UV_FORWARD;

            if (facing.HasFlag(BlockFacing.West))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[(south + 1) % 6], blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.East))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[(south + 4) % 6], blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.South))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[south], blockPos, Vector3Int.right, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.North))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[(south + 3) % 6], blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.Up))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[(south + 5) % 6], blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);
            }

            if (facing.HasFlag(BlockFacing.Down))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[(south + 2) % 6], blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);
            }
        }
    }
}

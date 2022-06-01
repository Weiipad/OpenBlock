using OpenBlock.Math;
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
        private int[] uvIndices = new int[6];
        public CommonBlock(int uvIndex): this(uvIndex, uvIndex, uvIndex) { }

        public CommonBlock(int axisUV, int sideUV): this(axisUV, sideUV, axisUV) { }

        public CommonBlock(int upUV, int sideUV, int downUV) : this(sideUV, upUV, sideUV, sideUV, downUV, sideUV) { }

        public CommonBlock(int rightUV, int upUV, int forwardUV, int leftUV, int downUV, int backUV)
        {
            uvIndices[BlockBuildHelper.UV_RIGHT] = rightUV;
            uvIndices[BlockBuildHelper.UV_UP] = upUV;
            uvIndices[BlockBuildHelper.UV_FORWARD] = forwardUV;
            uvIndices[BlockBuildHelper.UV_LEFT] = leftUV;
            uvIndices[BlockBuildHelper.UV_DOWN] = downUV;
            uvIndices[BlockBuildHelper.UV_BACK] = backUV;
        }

        public void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing)
        {
            if (facing == BlockFacing.None) return;

            int[] sideIndices = new int[] { BlockBuildHelper.UV_FORWARD, BlockBuildHelper.UV_RIGHT, BlockBuildHelper.UV_BACK, BlockBuildHelper.UV_LEFT };
            int sideOffset = 0;

            if (state.TryGetProperty("dir", out string dirStr))
            {
                Direction dir = (Direction)Enum.Parse(typeof(Direction), dirStr);
                switch (dir)
                {
                    case Direction.North:
                        sideOffset = 0;
                        break;
                    case Direction.East:
                        sideOffset = 3;
                        break;
                    case Direction.South:
                        sideOffset = 2;
                        break;
                    case Direction.West:
                        sideOffset = 1;
                        break;
                }
            }

            if (facing.HasFlag(BlockFacing.West))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[(3 + sideOffset) % 4]], blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.East))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[(1 + sideOffset) % 4]], blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.South))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[(2 + sideOffset) % 4]], blockPos, Vector3Int.right, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.North))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[sideOffset]], blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.Up))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[BlockBuildHelper.UV_UP], blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);
            }

            if (facing.HasFlag(BlockFacing.Down))
            {
                BlockBuildHelper.BuildPlane(ref builder, uvIndices[BlockBuildHelper.UV_DOWN], blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);
            }
        }
    }
}

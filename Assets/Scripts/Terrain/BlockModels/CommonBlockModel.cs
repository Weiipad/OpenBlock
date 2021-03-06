using OpenBlock.Math;
using OpenBlock.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.BlockModels
{
    public class CommonBlockModel : IBlockModel
    {
        private int[] uvIndices = new int[6];
        public CommonBlockModel(int uvIndex): this(uvIndex, uvIndex, uvIndex) { }

        public CommonBlockModel(int axisUV, int sideUV): this(axisUV, sideUV, axisUV) { }

        public CommonBlockModel(int upUV, int sideUV, int downUV) : this(sideUV, upUV, sideUV, sideUV, downUV, sideUV) { }

        public CommonBlockModel(int rightUV, int upUV, int forwardUV, int leftUV, int downUV, int backUV)
        {
            uvIndices[BlockModelBuildHelper.UV_RIGHT] = rightUV;
            uvIndices[BlockModelBuildHelper.UV_UP] = upUV;
            uvIndices[BlockModelBuildHelper.UV_FORWARD] = forwardUV;
            uvIndices[BlockModelBuildHelper.UV_LEFT] = leftUV;
            uvIndices[BlockModelBuildHelper.UV_DOWN] = downUV;
            uvIndices[BlockModelBuildHelper.UV_BACK] = backUV;
        }

        public void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing)
        {
            if (facing == BlockFacing.None) return;

            int[] sideIndices = new int[] { BlockModelBuildHelper.UV_FORWARD, BlockModelBuildHelper.UV_RIGHT, BlockModelBuildHelper.UV_BACK, BlockModelBuildHelper.UV_LEFT };
            int sideOffset = 0;

            if (state.TryGetProperty("dir", out Property prop))
            {
                Direction dir = (Direction)prop.GetByte();
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
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[(3 + sideOffset) % 4]], blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.East))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[(1 + sideOffset) % 4]], blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.South))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[(2 + sideOffset) % 4]], blockPos, Vector3Int.right, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.North))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[sideIndices[sideOffset]], blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.Up))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[BlockModelBuildHelper.UV_UP], blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);
            }

            if (facing.HasFlag(BlockFacing.Down))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[BlockModelBuildHelper.UV_DOWN], blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);
            }
        }
    }
}

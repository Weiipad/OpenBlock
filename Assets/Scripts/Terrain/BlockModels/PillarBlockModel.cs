using OpenBlock.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static OpenBlock.Terrain.BlockModels.BlockModelBuildHelper;

namespace OpenBlock.Terrain.BlockModels
{
    public class PillarBlockModel : IBlockModel
    {
        private int sideIndex, coreIndex;
        public PillarBlockModel(int coreIndex, int sideIndex)
        {
            this.sideIndex = sideIndex;
            this.coreIndex = coreIndex;
        }

        public void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing)
        {
            if (state.properties == null) throw new Exception("Wrong block" + state.id);

            int[] uvIndices = new int[6];
            UVDir[] uvDirs = new UVDir[6];

            if (state.TryGetProperty("axis", out string axisStr))
            {
                Axis axis = (Axis)Enum.Parse(typeof(Axis), axisStr);
                switch (axis)
                {
                    case Axis.X:
                        uvIndices[UV_LEFT] = uvIndices[UV_RIGHT] = coreIndex;
                        uvIndices[UV_FORWARD] = uvIndices[UV_BACK] = uvIndices[UV_UP] = uvIndices[UV_DOWN] = sideIndex;
                        uvDirs[UV_LEFT] = uvDirs[UV_RIGHT] = UVDir.Up;
                        uvDirs[UV_FORWARD] = UVDir.Left;
                        uvDirs[UV_DOWN] = uvDirs[UV_UP] = uvDirs[UV_BACK] = UVDir.Right;
                        break;
                    case Axis.Y:
                        uvIndices[UV_UP] = uvIndices[UV_DOWN] = coreIndex;
                        uvIndices[UV_FORWARD] = uvIndices[UV_BACK] = uvIndices[UV_RIGHT] = uvIndices[UV_LEFT] = sideIndex;
                        uvDirs[UV_DOWN] = uvDirs[UV_UP] = UVDir.Up;
                        uvDirs[UV_DOWN] = uvDirs[UV_FORWARD] = uvDirs[UV_UP] = uvDirs[UV_BACK] = UVDir.Up;
                        break;
                    case Axis.Z:
                        uvIndices[UV_FORWARD] = uvIndices[UV_BACK] = coreIndex;
                        uvIndices[UV_UP] = uvIndices[UV_DOWN] = uvIndices[UV_RIGHT] = uvIndices[UV_LEFT] = sideIndex;
                        uvDirs[UV_FORWARD] = uvDirs[UV_BACK] = UVDir.Up;
                        uvDirs[UV_RIGHT] = UVDir.Right;
                        uvDirs[UV_LEFT] = UVDir.Left;
                        uvDirs[UV_UP] = UVDir.Up;
                        uvDirs[UV_DOWN] = UVDir.Down;
                        break;
                }
            }

            if (facing.HasFlag(BlockFacing.West))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[UV_LEFT], uvDirs[UV_LEFT], blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.East))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[UV_RIGHT], uvDirs[UV_RIGHT], blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.South))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[UV_BACK], uvDirs[UV_BACK], blockPos, Vector3Int.right, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.North))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[UV_FORWARD], uvDirs[UV_FORWARD], blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.Up))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[UV_UP], uvDirs[UV_UP], blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);
            }

            if (facing.HasFlag(BlockFacing.Down))
            {
                BlockModelBuildHelper.BuildPlane(ref builder, uvIndices[UV_DOWN], uvDirs[UV_DOWN], blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);

            }
        }
    }
}
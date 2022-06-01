using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.Blocks
{
    public class GrassBlock : IBlock
    {
        public void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing)
        {
            if (facing == BlockFacing.None) return;

            
            Color color = new Color(0.125f, 0.9f, 0.125f, 1.0f);
            if (state.TryGetProperty("RGB", out string value))
            {
                uint colorCode = uint.Parse(value);
                color = new Color((byte)(colorCode >> 0) / 255.0f, (byte)(colorCode >> 8) / 255.0f, (byte)(colorCode >> 16) / 255.0f);
            }

            if (facing.HasFlag(BlockFacing.West))
            {
                BlockBuildHelper.BuildPlane(ref builder, 3, blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.East))
            {
                BlockBuildHelper.BuildPlane(ref builder, 3, blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.South))
            {
                BlockBuildHelper.BuildPlane(ref builder, 3, blockPos, Vector3Int.right, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.North))
            {
                BlockBuildHelper.BuildPlane(ref builder, 3, blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.Up))
            {
                BlockBuildHelper.BuildColoredPlane(ref builder, 0, color, blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);
            }

            if (facing.HasFlag(BlockFacing.Down))
            {
                BlockBuildHelper.BuildPlane(ref builder, 2, blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);
            }
        }
    }
}

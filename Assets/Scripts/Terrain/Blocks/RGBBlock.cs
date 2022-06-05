using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.Blocks
{
    public class RGBBlock : IBlock
    {
        public void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing)
        {
            if (facing == BlockFacing.None) return;

            uint colorCode = uint.Parse(state.properties["RGB"]);
            Color color = new Color((byte)(colorCode >> 0) / 255.0f, (byte)(colorCode >> 8) / 255.0f, (byte)(colorCode >> 16) / 255.0f);


            if (facing.HasFlag(BlockFacing.West))
            {
                BlockBuildHelper.BuildRGBPlane(ref builder, color, blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.East))
            {
                BlockBuildHelper.BuildRGBPlane(ref builder, color, blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.South))
            {
                BlockBuildHelper.BuildRGBPlane(ref builder, color, blockPos, Vector3Int.right, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.North))
            {
                BlockBuildHelper.BuildRGBPlane(ref builder, color, blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
            }

            if (facing.HasFlag(BlockFacing.Up))
            {
                BlockBuildHelper.BuildRGBPlane(ref builder, color, blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);
            }

            if (facing.HasFlag(BlockFacing.Down))
            {
                BlockBuildHelper.BuildRGBPlane(ref builder, color, blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);
            }
        }
    }
}

using OpenBlock.Utils;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class VoxLevel : ILevel
    {
        // size in chunk position unit.
        private Vector3Int size;
        private List<Chunk> loadedChunks;
        public VoxLevel(VoxReader reader)
        {
            //Ate main chunk header
            Debug.Assert(reader.ReadHeader().chunkId == "MAIN");

            var curHeader = reader.ReadHeader();
            if (curHeader.chunkId != "SIZE")
            {
                reader.SkipChunk(curHeader);
                curHeader = reader.ReadHeader();
            }
            // save pos
            var modelPos = reader.BaseStream.Position;
            // Find palette
            while (curHeader.chunkId != "RGBA" && !reader.IsEnd())
            {
                reader.SkipChunk(curHeader);
                curHeader = reader.ReadHeader();
            }
            Debug.Assert(curHeader.chunkId == "RGBA");

            var palette = reader.ReadPalette().ToArray();
            reader.BaseStream.Position = modelPos;
            // now it must be 'SIZE'
            var voxSize = reader.ReadSize();
            size = new Vector3Int(
                Mathf.CeilToInt(voxSize.x / (float)Chunk.SIZE), 
                Mathf.CeilToInt(voxSize.x / (float)Chunk.SIZE), 
                Mathf.CeilToInt(voxSize.x / (float)Chunk.SIZE)
            );
            loadedChunks = new List<Chunk>(size.x * size.y * size.z);

            var xyzi = reader.ReadHeader();
            Debug.Assert(xyzi.chunkId == "XYZI");
            foreach (var voxel in reader.ReadVoxels())
            {
                var color = palette[voxel.i - 1];
                SetBlock(BlockState.RGB(new Color(color.r / 255.0f, color.g / 255.0f, color.b / 255.0f)), new Vector3Int(voxel.x, voxel.z, voxel.y));
            }
        }

        public Chunk GetChunk(Vector3Int chunkPos)
        {
            if (!MathUtils.IsInCuboid(Vector3Int.zero, size, chunkPos)) return null;

            foreach (var chunk in loadedChunks)
            {
                if (chunk.chunkPos == chunkPos) return chunk;
            }

            Chunk ch = new Chunk(chunkPos);
            loadedChunks.Add(ch);
            return ch;
        }

        public void SetBlock(BlockState block, Vector3Int blockPos)
        {
            if (!MathUtils.IsInCuboid(Vector3Int.zero, size * Chunk.SIZE, blockPos)) return;
            var chunkPos = MathUtils.BlockPos2ChunkPos(blockPos);
            GetChunk(chunkPos).AddBlock(block, MathUtils.BlockPos2InternalPos(blockPos));
        }

        public BlockState GetBlock(Vector3Int blockPos)
        {
            if (!MathUtils.IsInCuboid(Vector3Int.zero, size * Chunk.SIZE, blockPos)) return BlockState.AIR;
            if (GetChunk(MathUtils.BlockPos2ChunkPos(blockPos)) is Chunk chunk)
            {
                return chunk.GetBlock(MathUtils.BlockPos2InternalPos(blockPos));
            }
            return BlockState.AIR;
        }

        public void SaveChunk(Chunk chunk)
        {

        }
    }
}
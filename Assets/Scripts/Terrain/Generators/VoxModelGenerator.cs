using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.Generators
{
    public class VoxModelGenerator : ITerrainGenerator
    {
        public int[,,] indices;
        private Color[] palette = new Color[256];
        private Vector3Int size;

        public VoxModelGenerator(string filePath)
        {
            using var file = File.OpenRead(filePath);
            Init(new VoxReader(file));
        }

        public VoxModelGenerator(VoxReader reader)
        {
            Init(reader);
        }

        private void Init(VoxReader reader)
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

            int idx = 0;
            foreach (var color in reader.ReadPalette())
            {
                palette[idx++] = new Color(color.r / 255.0f, color.g / 255.0f, color.b / 255.0f);
            }
            reader.BaseStream.Position = modelPos;
            // now it must be 'SIZE'
            var vSize = reader.ReadSize();
            size = new Vector3Int(vSize.x, vSize.y, vSize.z);
            indices = new int[size.x, size.y, size.z];

            var xyzi = reader.ReadHeader();
            Debug.Assert(xyzi.chunkId == "XYZI");
            foreach (var voxel in reader.ReadVoxels())
            {
                //var color = palette[voxel.i - 1];
                indices[voxel.x, voxel.z, voxel.y] = voxel.i;
                // SetBlock(BlockState.RGB(new Color(color.r / 255.0f, color.g / 255.0f, color.b / 255.0f)), new Vector3Int(voxel.x, voxel.z, voxel.y));
            }
        }

        public void GenerateTerrain(ref Chunk chunk)
        {
            var blockPos = MathUtils.ChunkPos2BlockPos(chunk.chunkPos);
            if (!MathUtils.IsInCuboid(Vector3Int.zero, size, blockPos)) return;
            if (!MathUtils.IsInCuboid(Vector3Int.zero, size, blockPos + Chunk.CUBE_SIZE)) return;

            for (int x = 0; x < Chunk.SIZE; ++x)
            {
                for (int y = 0; y < Chunk.SIZE; ++y)
                {
                    for (int z = 0; z < Chunk.SIZE; ++z)
                    {
                        int idx = indices[blockPos.x + x, blockPos.y + y, blockPos.z + z];
                        if (idx != 0) chunk.AddBlock(BlockState.RGB(palette[idx - 1]), new Vector3Int(x, y, z));
                    }
                }
            }
        }
    }
}

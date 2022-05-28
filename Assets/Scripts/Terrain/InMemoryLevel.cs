using OpenBlock.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class InMemoryLevel : ILevel
    {
        // size in chunk position unit.
        private Vector3Int size;
        private int seed;
        private List<Chunk> loadedChunks;

        private Vector2 samplePosition;
        public InMemoryLevel(int randomSeed)
        {
            seed = randomSeed;
            size = new Vector3Int(4, 4, 4);
            loadedChunks = new List<Chunk>(size.x * size.y * size.z);
            samplePosition = new Vector2(seed * size.x * Chunk.SIZE, seed * size.z * Chunk.SIZE);
        }

        public Chunk GetChunk(Vector3Int chunkPos)
        {
            if (!MathUtils.IsInCuboid(Vector3Int.zero, size, chunkPos)) return null;

            foreach (var chunk in loadedChunks)
            {
                if (chunk.chunkPos == chunkPos) return chunk;
            }

            return LoadChunk(chunkPos);
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

        
        public Chunk LoadChunk(Vector3Int chunkPos)
        {
            var c = new Chunk(chunkPos);
            InitTerrainData(ref c, chunkPos);
            loadedChunks.Add(c);
            return c;
        }

        private void InitTerrainData(ref Chunk chunk, Vector3Int chunkPos)
        {
            for (int x = 0; x < Chunk.SIZE; x++)
            {
                for (int z = 0; z < Chunk.SIZE; z++)
                {
                    int worldX = chunkPos.x * Chunk.SIZE + x;
                    int worldY = chunkPos.y * Chunk.SIZE;
                    int worldZ = chunkPos.z * Chunk.SIZE + z;
                    int worldHeight = GenerateHeight(worldX, 0, worldZ);
                    for (int y = 0; y < Chunk.SIZE && worldY <= worldHeight; y++)
                    {
                        if (worldY == worldHeight) 
                            chunk.AddBlock(BlockState.RGB(Color.green), new Vector3Int(x, y, z));
                        else
                            chunk.AddBlock(BlockState.RGB(ColorUtils.BROWN), new Vector3Int(x, y, z));

                        worldY = y + chunkPos.y * Chunk.SIZE;
                    }
                }
            }
        }

        private int GenerateHeight(int x, int y, int z)
        {
            return Mathf.FloorToInt(Mathf.PerlinNoise(x / (float)Chunk.SIZE, z / (float)Chunk.SIZE) * 10.0f + 12);
        }

        public void SaveChunk(Chunk chunk)
        {

        }
    }
}

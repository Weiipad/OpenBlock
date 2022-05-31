using OpenBlock.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain.Generators
{
    public class StandardGenerator : ITerrainGenerator
    {
        private Vector2 sampleOffset;
        public StandardGenerator(long seed)
        {
            var seedBytes = BitConverter.GetBytes(seed);
            sampleOffset = new Vector2(BitConverter.ToInt32(seedBytes, 0), BitConverter.ToInt32(seedBytes, 4));
            Debug.Log($"{sampleOffset}");
        }

        public void GenerateTerrain(ref Chunk chunk)
        {
            for (int x = 0; x < Chunk.SIZE; x++)
            {
                int worldX = chunk.chunkPos.x * Chunk.SIZE + x;
                for (int z = 0; z < Chunk.SIZE; z++)
                {
                    int worldZ = chunk.chunkPos.z * Chunk.SIZE + z;
                    int worldHeight = GetHeight(worldX, worldZ);
                    int worldY = chunk.chunkPos.y * Chunk.SIZE;
                    for (int y = 0; y < Chunk.SIZE && worldY <= worldHeight; y++)
                    {
                        if (worldY == worldHeight)
                            chunk.AddBlock(BlockState.RGB(Color.green), new Vector3Int(x, y, z));
                        else
                            chunk.AddBlock(BlockState.RGB(ColorUtils.BROWN), new Vector3Int(x, y, z));

                        worldY = y + chunk.chunkPos.y * Chunk.SIZE;
                    }
                }
            }
        }

        private int GetHeight(int x, int z)
        {
            return Mathf.FloorToInt(Mathf.PerlinNoise(x / (float)Chunk.SIZE, z / (float)Chunk.SIZE) * 10.0f + 12);
        }
    }
}

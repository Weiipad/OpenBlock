using OpenBlock.Math;
using OpenBlock.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Terrain.Generators
{
    public class SkyIslandGenerator : ITerrainGenerator
    {
        private int seed;

        public SkyIslandGenerator(int seed)
        {
            this.seed = seed;
        }

        public void GenerateTerrain(ref Chunk chunk)
        {
            var originPos = MathUtils.ChunkPos2BlockPos(chunk.chunkPos);
            float[,] heightMap = NoiseGen.PerlinHeightMap(Chunk.SIZE, Chunk.SIZE, seed, 20.0f, 3, 0.5f, 2, new Vector2(originPos.x, originPos.z));
            for (int x = 0; x < Chunk.SIZE; ++x)
            {
                for (int z = 0; z < Chunk.SIZE; ++z)
                {
                    var blockPosX = originPos.x + x;
                    var blockPosZ = originPos.z + z;

                    var inBound = blockPosX * blockPosX + blockPosZ * blockPosZ <= 1024;

                    if (!inBound) continue;

                    int min = Mathf.FloorToInt(Mathf.Sqrt(blockPosX * blockPosX + blockPosZ * blockPosZ) - 16);
                    int max = Mathf.FloorToInt(8.0f * heightMap[x, z] + 2);

                    for (int y = 0; y < Chunk.SIZE; ++y)
                    {
                        int blockPosY = originPos.y + y;
                        
                        bool inHeight = blockPosY >= min && blockPosY <= max;
                        if (inHeight)
                        {
                            if (blockPosY < 0)
                            {
                                chunk.AddBlock(BlockState.RGB(Color.gray), new Vector3Int(x, y, z));
                            }
                            else if (blockPosY < max - 1)
                            {
                                chunk.AddBlock(BlockState.RGB(ColorUtils.BROWN), new Vector3Int(x, y, z));
                            }
                            else
                            {
                                chunk.AddBlock(BlockState.RGB(Color.green), new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
            }
        }
    }
}
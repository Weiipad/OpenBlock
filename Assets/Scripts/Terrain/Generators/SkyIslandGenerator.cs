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
        private static int radius = 32;
        private static int sqrRadius = radius * radius;
        private float[,] heightMap;

        public SkyIslandGenerator(int seed)
        {
            this.seed = seed;
            heightMap = NoiseGen.PerlinHeightMap(2 * radius + 2, 2 * radius + 2, seed, 20.0f, 3, 0.5f, 2, Vector2.zero);
        }

        public void GenerateTerrain(ref Chunk chunk)
        {
            var originPos = MathUtils.ChunkPos2BlockPos(chunk.chunkPos);
            
            for (int x = 0; x < Chunk.SIZE; ++x)
            {
                for (int z = 0; z < Chunk.SIZE; ++z)
                {
                    var blockPosX = originPos.x + x;
                    var blockPosZ = originPos.z + z;

                    var sqrDistance = blockPosX * blockPosX + blockPosZ * blockPosZ;

                    var inBound = sqrDistance <= sqrRadius;

                    if (!inBound) continue;

                    int min = Mathf.FloorToInt(Mathf.Sqrt(sqrDistance) - 16);
                    int max = Mathf.FloorToInt(8.0f * heightMap[blockPosX + radius, blockPosZ + radius] + 2);

                    for (int y = 0; y < Chunk.SIZE; ++y)
                    {
                        int blockPosY = originPos.y + y;
                        
                        bool inHeight = blockPosY >= min && blockPosY <= max;
                        if (inHeight)
                        {
                            if (blockPosY < 0)
                            {
                                chunk.AddBlock(new BlockState(BlockId.Stone), new Vector3Int(x, y, z));
                            }
                            else if (blockPosY < max)
                            {
                                chunk.AddBlock(new BlockState(BlockId.Dirt), new Vector3Int(x, y, z));
                            }
                            else
                            {
                                chunk.AddBlock(new BlockState(BlockId.Grass), new Vector3Int(x, y, z));
                            }
                        }
                    }
                }
            }
        }
    }
}
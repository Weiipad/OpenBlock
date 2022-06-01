using OpenBlock.Utils;
using SimplexNoise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace OpenBlock.Terrain.Generators
{
    public class StandardGenerator : ITerrainGenerator
    {
        private Vector3 offset0, offset1, offset2;
        private float frequency = 0.025f;
        private float amplitude = 1f;
        public StandardGenerator(int seed)
        {
            Random.InitState(seed);
            offset0 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            offset1 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
            offset2 = new Vector3(Random.value * 1000, Random.value * 1000, Random.value * 1000);
        }

        public void GenerateTerrain(ref Chunk chunk)
        {
            for (int x = 0; x < Chunk.SIZE; x++)
            {
                int worldX = chunk.chunkPos.x * Chunk.SIZE + x;
                for (int z = 0; z < Chunk.SIZE; z++)
                {
                    int worldZ = chunk.chunkPos.z * Chunk.SIZE + z;
                    for (int y = 0; y < Chunk.SIZE; y++)
                    {
                        int worldY = chunk.chunkPos.y * Chunk.SIZE + y;
                        if (worldY < 0) chunk.AddBlock(BlockState.RGB(Color.black), new Vector3Int(x, y, z));
                        else if (worldY < 15) chunk.AddBlock(BlockState.RGB(ColorUtils.BROWN), new Vector3Int(x, y, z));
                        else if (worldY < 16) chunk.AddBlock(BlockState.RGB(Color.green), new Vector3Int(x, y, z));
                        else continue;
                    }
                }
            }
        }

        private int GetHeight(int x, int y, int z)
        {
            float x0 = (x + offset0.x) * frequency;
            float y0 = (y + offset0.y) * frequency;
            float z0 = (z + offset0.z) * frequency;

            float x1 = (x + offset1.x) * frequency * 2;
            float y1 = (y + offset1.y) * frequency * 2;
            float z1 = (z + offset1.z) * frequency * 2;

            float x2 = (x + offset2.x) * frequency / 4;
            float y2 = (y + offset2.y) * frequency / 4;
            float z2 = (z + offset2.z) * frequency / 4;

            float noise0 = Noise.Generate(x0, y0, z0) * amplitude;
            float noise1 = Noise.Generate(x1, y1, z1) * amplitude / 2;
            float noise2 = Noise.Generate(x2, y2, z2) * amplitude / 4;

            return Mathf.FloorToInt(noise0 + noise1 + noise2 + 10f);

        }
    }
}

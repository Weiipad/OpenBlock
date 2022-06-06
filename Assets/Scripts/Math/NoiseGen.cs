using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Math
{
    public static class NoiseGen
    {
        public static float[,] PerlinHeightMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];
            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            System.Random random = new System.Random(seed);

            Vector2[] octaveOffsets = new Vector2[octaves];
            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(0, 10000);
                float offsetY = random.Next(0, 10000);
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            float maxHeight = float.MinValue;
            float minHeight = float.MaxValue;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x + offset.x) / scale * frequency + octaveOffsets[i].x;
                        float sampleY = (y + offset.y) / scale * frequency + octaveOffsets[i].y;

                        float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2.0f - 1.0f;
                        noiseHeight += perlinValue * amplitude;

                        amplitude *= persistance;
                        frequency *= lacunarity;
                    }

                    if (noiseHeight < minHeight)
                    {
                        minHeight = noiseHeight;
                    }
                    else if (noiseHeight > maxHeight)
                    {
                        maxHeight = noiseHeight;
                    }


                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
                }
            }
            return noiseMap;
        }
    }
}

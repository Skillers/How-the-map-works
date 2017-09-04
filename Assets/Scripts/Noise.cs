using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise{

    public static float[,] GenerateNoiseMap(int mapWidth, int seed, int mapHeight, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffSets = new Vector2[octaves];

        for (int i = 0; i < octaves; i++)
        {
            float offSetX = prng.Next(-100000, 100000) + offset.x;
            float offSetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffSets[i] = new Vector2(offSetX, offSetY);
        }

        if (scale <= 0)
        {
            scale = 0.01f;
        }

        float maxNoiseHeigt = float.MinValue;
        float minNoiseHeigt = float.MaxValue;

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = x / scale * frequency + octaveOffSets[i].x;
                    float sampleY = y / scale * frequency + octaveOffSets[i].y;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if(noiseHeight > maxNoiseHeigt)
                {
                    maxNoiseHeigt = noiseHeight;
                }else if(noiseHeight < minNoiseHeigt)
                {
                    minNoiseHeigt = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeigt, maxNoiseHeigt, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}

using Code.Noise;
using UnityEngine;
using System;

public static class myDensity
{
    private static float[] points = new float[] { 1,1,1,
                                                  1,1,1,
                                                  1,1,1,
                                                                            1,1,1,
                                                                            1,-1,1,
                                                                            1,1,1,
                                                                                                1,1,1,
                                                                                                1,1,1,
                                                                                                1,1,1,
                                                  1};
    private static int width = 3;
    private static int height = 3;
    private static int length = 3;

    // Get index into points at the given position
    private static int GetIdx(int x, int y, int z, int width, int height, int length)
    {
        if (x >= width || y >= height || z >= length)
        {
            // Last value is a dummy
            return width * height * length;
        }
        if (x < 0 || y < 0 || z < 0)
        {
            // Last value is a dummy
            return width * height * length;
        }

        int idx = x + y * width + z * width * height;
        return idx;
    }

    public static float GetDensity(Vector3 pos)
    {

        float xPos = pos.x;
        float yPos = pos.y;
        float zPos = pos.z;

        

        // If position is out of points, return early
        if (xPos >= width || yPos >= height || zPos >= length)
        {
            return 1;
        }
        if (xPos < 0 || yPos < 0 || zPos < 0)
        {
            return 1;
        }

        // A bad test
        xPos -= .5f;
        yPos -= .5f;
        zPos -= .5f;

        /// Trilinear interpolation between nearest vals

        // Corner values
        float val0 = points[GetIdx(Mathf.FloorToInt(xPos), Mathf.FloorToInt(yPos), Mathf.FloorToInt(zPos), width, height, length)];
        float val1 = points[GetIdx(Mathf.CeilToInt(xPos), Mathf.FloorToInt(yPos), Mathf.FloorToInt(zPos), width, height, length)];
        float val2 = points[GetIdx(Mathf.CeilToInt(xPos), Mathf.CeilToInt(yPos), Mathf.FloorToInt(zPos), width, height, length)];
        float val3 = points[GetIdx(Mathf.FloorToInt(xPos), Mathf.CeilToInt(yPos), Mathf.FloorToInt(zPos), width, height, length)];
        float val4 = points[GetIdx(Mathf.FloorToInt(xPos), Mathf.FloorToInt(yPos), Mathf.CeilToInt(zPos), width, height, length)];
        float val5 = points[GetIdx(Mathf.CeilToInt(xPos), Mathf.FloorToInt(yPos), Mathf.CeilToInt(zPos), width, height, length)];
        float val6 = points[GetIdx(Mathf.CeilToInt(xPos), Mathf.CeilToInt(yPos), Mathf.CeilToInt(zPos), width, height, length)];
        float val7 = points[GetIdx(Mathf.FloorToInt(xPos), Mathf.CeilToInt(yPos), Mathf.CeilToInt(zPos), width, height, length)];

        // Proportion along each axis. Easy since points are spaced along integer values!
        float xDist = xPos % 1.0f;
        float yDist = yPos % 1.0f;
        float zDist = zPos % 1.0f;

        float lowClose = Mathf.Lerp(val0, val1, xDist);
        float lowFar = Mathf.Lerp(val3, val2, xDist);

        float highClose = Mathf.Lerp(val4, val5, xDist);
        float highFar = Mathf.Lerp(val7, val6, xDist);

        float lowMid = Mathf.Lerp(lowClose, lowFar, yDist);
        float highMid = Mathf.Lerp(highClose, highFar, yDist);

        float final = Mathf.Lerp(lowMid, highMid, zDist);

        return final;
    }
}


public static class glm
{
    public static float Sphere(Vector3 worldPosition, Vector3 origin, float radius)
    {
        return Vector3.Magnitude(worldPosition - origin) - radius;
    }

    public static float Cuboid(Vector3 worldPosition, Vector3 origin, Vector3 halfDimensions)
    {
        Vector3 local_pos = worldPosition - origin;
        Vector3 pos = local_pos;

        Vector3 d = new Vector3(Mathf.Abs(pos.x), Mathf.Abs(pos.y), Mathf.Abs(pos.z)) - halfDimensions;
        float m = Mathf.Max(d.x, Mathf.Max(d.y, d.z));
        return Mathf.Min(m, Vector3.Magnitude(d.magnitude > 0 ? d : Vector3.zero));
    }

    public static float FractalNoise(int octaves, float frequency, float lacunarity, float persistence, Vector2 position)
    {
        float SCALE = 1.0f / 128.0f;
        Vector2 p = position * SCALE;
        float noise = 0.0f;

        float amplitude = 1.0f;
        p *= frequency;

        for (int i = 0; i < octaves; i++)
        {
            noise += Noise.Perlin(p.x, p.y) * amplitude;
            p *= lacunarity;
            amplitude *= persistence;
        }

        // move into [0, 1] range
        return 0.5f + (0.5f * noise);
    }


    private static float[] points = new float[] { 1,1,1,
                                                  1,1,1,
                                                  1,1,1,
                                                                            1,1,1,
                                                                            1,-1,1,
                                                                            1,1,1,
                                                                                                1,1,1,
                                                                                                1,1,1,
                                                                                                1,1,1,
                                                  1};
    private static int width = 3;
    private static int height = 3;
    private static int length = 3;

    // Get index into points at the given position
    private static int GetIdx(int x, int y, int z, int width, int height, int length)
    {
        if (x >= width || y >= height || z >= length)
        {
            // Last value is a dummy
            return width * height * length;
        }
        if (x < 0 || y < 0 || z < 0)
        {
            // Last value is a dummy
            return width * height * length;
        }

        int idx = x + y * width + z * width * height;
        return idx;
    }

    public static float Density_Func(Vector3 worldPosition)
    {
        //float MAX_HEIGHT = 20.0f;
        //float noise = FractalNoise(4, 0.5343f, 2.2324f, 0.68324f, new Vector2(worldPosition.x, worldPosition.z));
        //float terrain = worldPosition.y - (MAX_HEIGHT * noise);

        //float cube = Cuboid(worldPosition, new Vector3(-4.0f, 10.0f, -4.0f), new Vector3(12.0f, 12.0f, 12.0f));
        //float sphere = Sphere(worldPosition, new Vector3(15.0f, 2.5f, 1.0f), 16.0f);

        //return Mathf.Max(-cube, Mathf.Min(sphere, terrain));
        Vector3 center = new Vector3(3, 3, 3);
        float distToCenter = Vector3.Magnitude(center - worldPosition);
        return distToCenter - 1;

        return myDensity.GetDensity(worldPosition);
    }
}
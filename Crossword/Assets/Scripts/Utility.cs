using System;
using UnityEngine;

public static class Utility
{
    public static Vector3[] GetCirclePositions(Vector3 center, int divisions, float radius)
    {
        float angle = 360f / divisions;
        float denominator = 180f / Mathf.PI;
        Vector3[] pos = new Vector3[divisions];
        for (int i = 0; i < divisions; i++)
        {
            float x = center.x + (radius * Mathf.Sin((angle * i) / denominator));
            float y = center.y + (radius * Mathf.Cos((angle * i) / denominator));
            float z = center.z;

            pos[i] = new Vector3(x, y, z);
        }

        return pos;
    }

    public static string[] SortByLengthDesc(string[] strs)
    {
        Array.Sort(strs, (x, y) => y.Length.CompareTo(x.Length));
        return strs;
    }

    public static int GetSumOfLengths(string[] strs)
    {
        int sum = 0;
        foreach (string str in strs)
        {
            sum += str.Length;
        }

        return sum;
    }

    public static Vector2 GetTileSize(Vector2 perimeter, IntPoint tileArrayLengths)
    {
        float hor = perimeter.x / tileArrayLengths.X;
        float ver = perimeter.y / tileArrayLengths.Y;
        return new Vector2(hor, ver);
    }
}

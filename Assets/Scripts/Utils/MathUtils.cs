using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static Vector3Int GetBlockPos(Vector3 point, Vector3 surfaceNormal)
    {
        surfaceNormal.Normalize();
        point -= 0.1f * surfaceNormal;
        return AsBlockPos(point);
    }

    public static Vector3Int AsBlockPos(Vector3 point)
    {
        return new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), Mathf.FloorToInt(point.z));
    }
}

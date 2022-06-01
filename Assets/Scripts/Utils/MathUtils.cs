using OpenBlock.Terrain;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    public static float SqrtTwoOverTwo = Mathf.Sqrt(2) / 2.0f;

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

    public static Vector3Int ChunkPos2BlockPos(Vector3Int chunkPos)
    {
        return chunkPos * Chunk.SIZE;
    }

    public static Vector3Int BlockPos2ChunkPos(Vector3Int blockPos) => BlockPos2ChunkPos(blockPos.x, blockPos.y, blockPos.z);

    public static Vector3Int BlockPos2ChunkPos(int x, int y, int z)
    {
        return new Vector3Int(
            Mathf.FloorToInt(x / (float)Chunk.SIZE),
            Mathf.FloorToInt(y / (float)Chunk.SIZE),
            Mathf.FloorToInt(z / (float)Chunk.SIZE)
        );
    }

    public static Vector3Int BlockPos2InternalPos(Vector3Int blockPos)
    {
        blockPos.x = blockPos.x >= 0 ? blockPos.x : Mathf.Abs(Chunk.SIZE - Mathf.Abs(blockPos.x) % Chunk.SIZE);
        blockPos.y = blockPos.y >= 0 ? blockPos.y : Mathf.Abs(Chunk.SIZE - Mathf.Abs(blockPos.y) % Chunk.SIZE);
        blockPos.z = blockPos.z >= 0 ? blockPos.z : Mathf.Abs(Chunk.SIZE - Mathf.Abs(blockPos.z) % Chunk.SIZE);
        return new Vector3Int(blockPos.x % Chunk.SIZE, blockPos.y % Chunk.SIZE, blockPos.z % Chunk.SIZE);
    }

    public static Vector3Int InternalPos2BlockPos(int x, int y, int z, Vector3Int chunkPos)
    {
        var ans = ChunkPos2BlockPos(chunkPos);
        ans.x += x;
        ans.y += y;
        ans.z += z;
        return ans;
    }

    public static int InternalPos2BlockPos(int iPos, int chunkPos)
    {
        return chunkPos * Chunk.SIZE + iPos;
    }

    public static bool IsInCuboid(Vector3Int start, Vector3Int end, Vector3Int blockPos)
    {
        var (xMin, xMax) = (start.x < end.x) ? (start.x, end.x) : (end.x, start.x);
        var (yMin, yMax) = (start.y < end.y) ? (start.y, end.y) : (end.y, start.y);
        var (zMin, zMax) = (start.z < end.z) ? (start.z, end.z) : (end.z, start.z);
        return Between(blockPos.x, xMin, xMax) && Between(blockPos.y, yMin, yMax) && Between(blockPos.z, zMin, zMax);
    }

    public static bool Between(int num, int min, int max)
    {
        return num >= min && num <= max;
    }

    public static (int, int) FindMinMax(int a, int b) => a < b ? (a, b) : (b, a);
}

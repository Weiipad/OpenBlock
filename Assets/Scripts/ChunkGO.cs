using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenBlock.Chunks;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class ChunkGO : MonoBehaviour
{
    // x: East & West
    // y: Up & Down
    // z: North & South
    [System.Flags]
    public enum FaceDirection: byte
    {
        None,
        East = 1, West = 2,
        South = 4, North = 8,
        Up = 16, Down = 32,
    }

    private MeshFilter meshFilter;
    private MeshCollider meshCollider;

    private bool[,,] testTerrain;

    private void Awake()
    {
        meshFilter = GetComponent<MeshFilter>();
        meshCollider = GetComponent<MeshCollider>();
        testTerrain = new bool[Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE, Chunk.CHUNK_SIZE];
        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
            {
                int rand = Random.Range(3, 6);
                for (int y = 0; y < rand; y++) testTerrain[x, y, z] = true;
            }
        }
        BuildChunkModel();
    }

    public void BuildChunkModel(/*Chunk chunk*/)
    {
        Mesh chunkMesh = new Mesh();
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        for (int x = 0; x < Chunk.CHUNK_SIZE; x++)
        {
            for (int y = 0; y < Chunk.CHUNK_SIZE; y++)
            {
                for (int z = 0; z < Chunk.CHUNK_SIZE; z++)
                {
                    if (testTerrain[x, y, z])
                    {
                        FaceDirection dirLayer = 0;
                        if (x - 1 < 0 || !testTerrain[x - 1, y, z]) dirLayer |= FaceDirection.West;
                        if (x + 1 >= Chunk.CHUNK_SIZE || !testTerrain[x + 1, y, z]) dirLayer |= FaceDirection.East;
                        if (y - 1 < 0 || !testTerrain[x, y - 1, z]) dirLayer |= FaceDirection.Down;
                        if (y + 1 >= Chunk.CHUNK_SIZE || !testTerrain[x, y + 1, z]) dirLayer |= FaceDirection.Up;
                        if (z - 1 < 0 || !testTerrain[x, y, z - 1]) dirLayer |= FaceDirection.South;
                        if (z + 1 >= Chunk.CHUNK_SIZE || !testTerrain[x, y, z + 1]) dirLayer |= FaceDirection.North;
                        BuildBlock(ref vertices, ref triangles, new Vector3Int(x, y, z), dirLayer);
                    }
                }
            }
        }

        chunkMesh.vertices = vertices.ToArray();
        chunkMesh.triangles = triangles.ToArray();
        chunkMesh.RecalculateNormals();
        meshFilter.mesh = chunkMesh;
        meshCollider.sharedMesh = chunkMesh;
    }

    public void BuildBlock(ref List<Vector3> vertices, ref List<int> indices, Vector3Int blockPos, FaceDirection dirLayer)
    {
        if ((dirLayer & FaceDirection.East) != 0) 
            BuildFace(ref vertices, ref indices, blockPos + Vector3Int.right, Vector3Int.forward, Vector3Int.up);
        if ((dirLayer & FaceDirection.West) != 0)
            BuildFace(ref vertices, ref indices, blockPos + Vector3Int.forward, Vector3Int.back, Vector3Int.up);
        if ((dirLayer & FaceDirection.South) != 0) 
            BuildFace(ref vertices, ref indices, blockPos, Vector3Int.right, Vector3Int.up);
        if ((dirLayer & FaceDirection.North) != 0)
            BuildFace(ref vertices, ref indices, blockPos + Vector3Int.right + Vector3Int.forward, Vector3Int.left, Vector3Int.up);
        if ((dirLayer & FaceDirection.Up) != 0) 
            BuildFace(ref vertices, ref indices, blockPos + Vector3Int.up, Vector3Int.right, Vector3Int.forward);    
        if ((dirLayer & FaceDirection.Down) != 0) 
            BuildFace(ref vertices, ref indices, blockPos + Vector3Int.forward, Vector3Int.right, Vector3Int.back);
    }

    /*
     *  b--------------o+a+b
     *  |               /|
     *  |            /   | 
     *  |         /      |
     *  |      /         |
     *  |   /            |
     *  |/               |
     *  o----------------a
     * 
     */

    public void BuildFace(ref List<Vector3> vertices, ref List<int> indices, Vector3 o, Vector3 a, Vector3 b)
    {
        int startVertex = vertices.Count;

        vertices.Add(o);
        vertices.Add(o + a);
        vertices.Add(o + b);
        vertices.Add(o + a + b);

        indices.AddRange(new int[] { startVertex + 0, startVertex + 3, startVertex + 1 });
        indices.AddRange(new int[] { startVertex + 0, startVertex + 2, startVertex + 3 });

    }

    private void OnBecameVisible()
    {
        Debug.Log("Chunk visibled");
    }

    private void OnBecameInvisible()
    {
        Debug.Log("Chunk invisibled");
    }
}

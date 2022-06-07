using OpenBlock.Terrain.BlockModels;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Terrain
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider))]
    public class ChunkObj : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private MeshRenderer meshRenderer;
        private MeshCollider meshCollider;

        [SerializeField]
        private Material opaque, rgb, transparent;

        private Mesh chunkMesh;
        private ChunkMeshBuilder meshBuilder;

        [HideInInspector]
        public Vector3Int chunkPos;

        public bool isVisible { get; private set; }
        private void Awake()
        {
            chunkMesh = new Mesh();
            chunkMesh.name = "Procedual Chunk";
            meshBuilder = new ChunkMeshBuilder();
            meshFilter = GetComponent<MeshFilter>();
            meshRenderer = GetComponent<MeshRenderer>();
            meshCollider = GetComponent<MeshCollider>();
        }

        public void Rebuild(Chunk chunk)
        {
            transform.position = chunk.chunkPos * Chunk.SIZE;
            chunkPos = chunk.chunkPos;
            meshBuilder.Clear();

            for (int x = 0; x < Chunk.SIZE; x++)
            {
                for (int z = 0; z < Chunk.SIZE; z++)
                {
                    for (int y = 0; y < Chunk.SIZE; y++)
                    {
                        var pos = new Vector3Int(x, y, z);
                        var state = chunk.GetBlock(pos);

                        if (state.id == BlockId.Air) continue;

                        var facing = BlockFacing.None;
                        if (!chunk.ExistBlockInternal(x - 1, y, z)) 
                            facing |= BlockFacing.West;

                        if (!chunk.ExistBlockInternal(x + 1, y, z)) 
                            facing |= BlockFacing.East;

                        if (!chunk.ExistBlockInternal(x, y - 1, z)) 
                            facing |= BlockFacing.Down;

                        if (!chunk.ExistBlockInternal(x, y + 1, z)) 
                            facing |= BlockFacing.Up;

                        if (!chunk.ExistBlockInternal(x, y, z - 1)) 
                            facing |= BlockFacing.South;

                        if (!chunk.ExistBlockInternal(x, y, z + 1)) 
                            facing |= BlockFacing.North;

                        BlockRegistry.blockModels[state.id].BuildModel(state, ref meshBuilder, pos, facing);
                    }
                }
            }

            meshBuilder.Build(ref chunkMesh, out int oidx, out int cidx, out int tidx);
            int materialCount = 0;
            if (oidx != -1) materialCount++;
            if (cidx != -1) materialCount++;
            if (tidx != -1) materialCount++;

            Material[] materials = new Material[materialCount];
            if (oidx != -1) materials[oidx] = opaque;
            if (cidx != -1) materials[cidx] = rgb;
            if (tidx != -1) materials[tidx] = transparent;

            meshFilter.mesh = chunkMesh;
            meshCollider.sharedMesh = chunkMesh;
            meshRenderer.materials = materials;
        }

        private void OnBecameInvisible()
        {
            isVisible = false;
        }

        private void OnBecameVisible()
        {
            isVisible = true;
        }
    }
}

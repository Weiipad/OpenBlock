using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using OpenBlock.Terrain.Generators;

namespace OpenBlock.Terrain
{
    public class WorldObj : MonoBehaviour
    {
        [SerializeField]
        private ChunkObj chunkObjPrefab;
        [SerializeField]
        private Transform playerTransform;


        // public ILevel level { get; private set; }
        public Level level { get; private set; }
        private List<ChunkObj> chunkObjPool;

        private void Awake()
        {
            level = new Level(new SkyIslandGenerator((int)System.DateTime.Now.Ticks));
            level.onChunkNeedRebuild += OnChunkNeedRebuild;
            chunkObjPool = new List<ChunkObj>();
        }

        public void OnChunkNeedRebuild(Chunk chunk)
        {
            foreach (var chunkObj in chunkObjPool)
            {
                if (chunkObj.chunkPos == chunk.chunkPos)
                {
                    chunkObj.Rebuild(chunk);
                }
            }
        }

        private void Start()
        {
            if (level.GetChunk(Vector3Int.zero) is Chunk chunk)
            {
                var chunkObj = Instantiate(chunkObjPrefab);
                chunkObj.transform.SetParent(transform);
                chunkObj.Rebuild(chunk);
                chunkObjPool.Add(chunkObj);
            }
        }

        private void Update()
        {
            var playerChunkPos = MathUtils.BlockPos2ChunkPos(MathUtils.AsBlockPos(playerTransform.position));
            CheckAndGenerateChunk(playerChunkPos);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.forward);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.back);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.left);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.right);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.up);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.down);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.down + Vector3Int.forward);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.down + Vector3Int.left);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.down + Vector3Int.right);
            CheckAndGenerateChunk(playerChunkPos + Vector3Int.down + Vector3Int.back);
        }

        private void LateUpdate()
        {
            level.CheckChunks();
        }

        private void CheckAndGenerateChunk(Vector3Int chunkPos)
        {
            if (level.GetChunk(chunkPos) is Chunk chunk)
            {
                bool playerChunkLoaded = false;
                foreach (var chunkObj in chunkObjPool)
                {
                    if (chunkObj.chunkPos == chunk.chunkPos)
                    {
                        playerChunkLoaded = true;
                        break;
                    }
                }

                if (!playerChunkLoaded)
                {
                    var chunkObj = Instantiate(chunkObjPrefab);
                    chunkObj.transform.SetParent(transform);
                    chunkObj.Rebuild(chunk);
                    chunkObjPool.Add(chunkObj);
                }
            }
        }

        private void OnDisable()
        {
            level.onChunkNeedRebuild -= OnChunkNeedRebuild;
        }
    }
}

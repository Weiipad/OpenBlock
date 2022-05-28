using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class WorldObj : MonoBehaviour
    {
        [SerializeField]
        private ChunkObj chunkObjPrefab;
        [SerializeField]
        private Transform playerTransform;


        public ILevel level { get; private set; }
        private List<ChunkObj> chunkObjPool;
        private int activeSplitIdx;

        private void Awake()
        {
            level = new InMemoryLevel(123456);
            chunkObjPool = new List<ChunkObj>();
            activeSplitIdx = 0;
        }

        private void Start()
        {
            if (level.GetChunk(Vector3Int.zero) is Chunk chunk)
            {
                var chunkObj = Instantiate(chunkObjPrefab);
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
                    chunkObj.Rebuild(chunk);
                    chunkObjPool.Add(chunkObj);
                }
            }
        }
    }
}

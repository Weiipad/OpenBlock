using OpenBlock.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class Level
    {
        private readonly static Vector3Int[] NEIBOUR_CHUNK_POS = new Vector3Int[]
        {
            Vector3Int.up, Vector3Int.right, Vector3Int.forward, Vector3Int.down, Vector3Int.left,  Vector3Int.back
        };

        public System.Action<Chunk> onChunkNeedRebuild;

        private Queue<Chunk> recycledChunks;
        private List<Chunk> loadedChunks;
        private ITerrainGenerator generator;

        public Level(ITerrainGenerator generator)
        {
            recycledChunks = new Queue<Chunk>();
            loadedChunks = new List<Chunk>();
            this.generator = generator;
        }

        public void CheckChunks()
        {
            foreach (var chunk in loadedChunks)
            {
                if (chunk.needRebuild)
                {
                    onChunkNeedRebuild?.Invoke(chunk);
                    chunk.needRebuild = false;
                }
            }
        }

        public BlockState GetBlock(Vector3Int blockPos)
        {
            if (GetChunk(MathUtils.BlockPos2ChunkPos(blockPos)) is Chunk chunk)
            {
                return chunk.GetBlock(MathUtils.BlockPos2InternalPos(blockPos));
            }
            return BlockState.AIR;
        }

        public void DestroyBlock(Vector3Int blockPos)
        {
            if (GetChunk(MathUtils.BlockPos2ChunkPos(blockPos)) is Chunk chunk)
            {
                chunk.DestroyBlock(MathUtils.BlockPos2InternalPos(blockPos));
            }
        }

        public void AddBlock(BlockState state, Vector3Int blockPos)
        {
            if (GetChunk(MathUtils.BlockPos2ChunkPos(blockPos)) is Chunk chunk)
            {
                chunk.AddBlock(state, MathUtils.BlockPos2InternalPos(blockPos));
            }
        }

        public Chunk GetChunk(Vector3Int chunkPos)
        {
            foreach (var chunk in loadedChunks)
            {
                if (chunk.chunkPos == chunkPos)
                {
                    return chunk;
                }
            }
            return LoadChunk(chunkPos);
        }

        private Chunk LoadChunk(Vector3Int chunkPos)
        {
            if (IsChunkLoaded(chunkPos)) return null;

            Chunk ch = null;
            if (recycledChunks.Count != 0)
            {
                ch = recycledChunks.Dequeue();
            }
            else
            {
                ch = new Chunk();
            }
            ch.Reset(chunkPos);

            generator.GenerateTerrain(ref ch);
            #region Link New Chunk
            foreach (var chunk in loadedChunks)
            {
                for (int i = 0; i < NEIBOUR_CHUNK_POS.Length; ++i)
                {
                    if (chunk.chunkPos == chunkPos + NEIBOUR_CHUNK_POS[i])
                    {
                        ch.neighbours[i] = chunk;
                        chunk.neighbours[(i + 3) % 6] = ch;
                    }
                }
            }
            #endregion
            loadedChunks.Add(ch);

            foreach (var neighbour in ch.neighbours)
            {
                if (neighbour != null)
                {
                    neighbour.needRebuild = true;
                }
            }
            
            return ch;
        }

        private bool IsChunkLoaded(Vector3Int chunkPos)
        {
            foreach (var chunk in loadedChunks)
            {
                if (chunk.chunkPos == chunkPos) return true;
            }
            return false;
        }
    }
}

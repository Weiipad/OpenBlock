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

        public System.Action<Chunk> onChunkLoaded;

        private Stack<Chunk> recycledChunks;
        private List<Chunk> loadedChunks;
        private ITerrainGenerator generator;

        public Level(ITerrainGenerator generator)
        {
            recycledChunks = new Stack<Chunk>();
            loadedChunks = new List<Chunk>();
            this.generator = generator;
        }

        public BlockState GetBlock(Vector3Int blockPos)
        {
            if (GetChunk(MathUtils.BlockPos2ChunkPos(blockPos)) is Chunk chunk)
            {
                return chunk.GetBlock(MathUtils.BlockPos2InternalPos(blockPos));
            }
            return BlockState.AIR;
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
                ch = recycledChunks.Pop();
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
            onChunkLoaded?.Invoke(ch);
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

using System.Collections.Generic;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public abstract class AbstractLevel : ILevel
    {
        public BlockState GetBlock(Vector3Int worldBlockPos)
        {
            throw new System.NotImplementedException();
        }

        public Chunk GetChunk(Vector3Int chunkPos)
        {
            throw new System.NotImplementedException();
        }

        public void SaveChunk(Chunk chunk)
        {
            throw new System.NotImplementedException();
        }
    }
}

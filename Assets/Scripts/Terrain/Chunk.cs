using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class Chunk
    {
        #region Temp Tests
        public static Chunk SampleChunk;
        static Chunk()
        {
            SampleChunk = new Chunk(new Vector3Int(0, 0, 0));
            Vector3Int[] xAxis = new Vector3Int[16];
            Vector3Int[] yAxis = new Vector3Int[16];
            Vector3Int[] zAxis = new Vector3Int[16];

            for (int i = 0; i < SIZE; i++)
            {
                xAxis[i] = new Vector3Int(i, 0, 0);
                yAxis[i] = new Vector3Int(0, i, 0);
                zAxis[i] = new Vector3Int(0, 0, i);
            }
            SampleChunk.BatchedAddBlock(BlockState.RGB(Color.red), xAxis);
            SampleChunk.BatchedAddBlock(BlockState.RGB(Color.green), yAxis);
            SampleChunk.BatchedAddBlock(BlockState.RGB(Color.blue), zAxis);
            SampleChunk.AddBlock(BlockState.RGB(Color.gray), Vector3Int.zero);
        }
        #endregion

        public const int SIZE = 16;

        public Vector3Int chunkPos;

        private readonly int[,,] indices = new int[16, 16, 16];
        private List<BlockState> blockStates = new List<BlockState>();

        public Chunk up, down, left, right, forward, back;

        public Chunk(Vector3Int chunkPos)
        {
            this.chunkPos = chunkPos;
            blockStates.Add(BlockState.AIR);
        }

        private bool IsLastSameBlock(int idx)
        {
            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    for (int z = 0; z < SIZE; z++)
                    {
                        if (indices[x, y, z] == idx) return false;
                    }
                }
            }
            return true;
        }

        private void RemovePalette(int idx)
        {
            if (idx == 0) return;
            var lastIdx = blockStates.Count - 1;
            var temp = blockStates[idx];
            blockStates[idx] = blockStates[lastIdx];
            blockStates[lastIdx] = temp;

            for (int x = 0; x < SIZE; x++)
            {
                for (int y = 0; y < SIZE; y++)
                {
                    for (int z = 0; z < SIZE; z++)
                    {
                        if (indices[x, y, z] == lastIdx) indices[x, y, z] = idx;
                    }
                }
            }

            blockStates.RemoveAt(lastIdx);
        }

        public void RemoveBlock(Vector3Int pos)
        {
            if (indices[pos.x, pos.y, pos.z] == 0) return;
            if (IsLastSameBlock(indices[pos.x, pos.y, pos.z]))
            {
                RemovePalette(indices[pos.x, pos.y, pos.z]);
            }
        }

        public bool ExistBlock(Vector3Int pos)
        {
            return ExistBlock(pos.x, pos.y, pos.z);
        }

        public bool ExistBlock(int x, int y, int z)
        {
            return indices[x, y, z] != 0;
        }

        public BlockState GetBlock(Vector3Int pos)
        {
            if (pos.x < 0 || pos.y < 0 || pos.z < 0) return null;
            if (pos.x >= SIZE || pos.y >= SIZE || pos.z >= SIZE) return null;

            return blockStates[indices[pos.x, pos.y, pos.z]];
        }

        public void AddBlock(BlockState block, Vector3Int pos)
        {
            int idx = blockStates.FindIndex(state => block.Equals(state));
            if (idx == -1)
            {
                idx = blockStates.Count;
                blockStates.Add(block);
            }
            indices[pos.x, pos.y, pos.z] = idx;
        }

        public void BatchedAddBlock(BlockState block, params Vector3Int[] positions)
        {
            int idx = blockStates.FindIndex(state => block.Equals(state));
            if (idx == -1)
            {
                idx = blockStates.Count;
                blockStates.Add(block);
            }

            foreach (var pos in positions)
            {
                indices[pos.x, pos.y, pos.z] = idx;
            }
        }
    }
}

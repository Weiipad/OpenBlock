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
        public static readonly Vector3Int CUBE_SIZE = new Vector3Int(SIZE, SIZE, SIZE);

        public const int NEIGHBOUR_COUNT = 6;

        public const int NEIGHBOUR_UP = 0;
        public const int NEIGHBOUR_RIGHT = 1;
        public const int NEIGHBOUR_FORWARD = 2;
        public const int NEIGHBOUR_DOWN = 3;
        public const int NEIGHBOUR_LEFT = 4;
        public const int NEIGHBOUR_BACK = 5;

        public Vector3Int chunkPos;

        private readonly int[,,] indices = new int[16, 16, 16];
        private List<BlockState> blockStates = new List<BlockState>();

        public Chunk[] neighbours = new Chunk[NEIGHBOUR_COUNT];

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
        public void SetNeighbour(int neighbourIndex, Chunk neighbour)
        {
            neighbours[neighbourIndex] = neighbour;
        }
        public void Reset()
        {
            for (int i = 0; i < NEIGHBOUR_COUNT; ++i)
            {
                neighbours[i] = null;
            }

            for (int x = 0; x < SIZE; ++x)
            {
                for (int y = 0; y < SIZE; ++y)
                {
                    for (int z = 0; z < SIZE; ++z)
                    {
                        indices[x, y, z] = 0;
                    }
                }
            }
        }
        public void ClearPalette()
        {
            blockStates.Clear();
            blockStates.Add(BlockState.AIR);
        }
        public void RemoveBlock(Vector3Int pos)
        {
            if (indices[pos.x, pos.y, pos.z] == 0) return;
            if (IsLastSameBlock(indices[pos.x, pos.y, pos.z]))
            {
                RemovePalette(indices[pos.x, pos.y, pos.z]);
            }
        }

        public bool ExistBlockInternal(Vector3Int pos) => ExistBlockInternal(pos.x, pos.y, pos.z);

        public bool ExistBlockInternal(int x, int y, int z)
        {
            var blockPos = MathUtils.InternalPos2BlockPos(x, y, z, chunkPos);
            if (Contains(blockPos)) return indices[x, y, z] != 0;
            foreach (Chunk neighbour in neighbours)
            {
                if (neighbour == null) continue;
                if (neighbour.Contains(blockPos)) return neighbour.ExistBlock(blockPos);
            }
            return false;
        }

        public bool ExistBlock(Vector3Int blockPos)
        {
            return ExistBlockInternal(blockPos - chunkPos * SIZE);
        }

        public bool Contains(Vector3Int blockPos)
        {
            var origin = MathUtils.ChunkPos2BlockPos(chunkPos);
            return MathUtils.IsInCuboid(origin, origin + CUBE_SIZE - Vector3Int.one, blockPos);
        }

        public BlockState GetBlock(Vector3Int pos)
        {
            if (!MathUtils.IsInCuboid(Vector3Int.zero, CUBE_SIZE, pos)) return null;

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Chunks
{
    public class Region
    {
        public const int REGION_SIZE = 8;
        public const int REGION_BLOCK_COUNT = REGION_SIZE * ChunkData.CHUNK_SIZE;

        private int x, y, z;
        private ChunkData[,,] chunks = new ChunkData[REGION_SIZE, REGION_SIZE, REGION_SIZE];

        public ChunkData this[int x, int y, int z] => GetChunk(x, y, z);

        public Region(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Region(string path, string fileName)
        {
            string[] split = fileName.Split('.');
            if (split[0] != "r") throw new Exception("Maybe not a region file");
            x = int.Parse(split[1]);
            y = int.Parse(split[2]);
            z = int.Parse(split[3]);
            using var file = new FileStream(path + Path.DirectorySeparatorChar + fileName, FileMode.OpenOrCreate);
            using var reader = new BinaryReader(file);
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                int cx = reader.ReadInt32();
                int cy = reader.ReadInt32();
                int cz = reader.ReadInt32();
                chunks[cx, cy, cz] = new ChunkData(reader);
            }
        }

        public void SetBlock(BlockInfo info, int x, int y, int z)
        {
            if (!InRegion(x, y, z)) return;

            var (cx, cy, cz, bx, by, bz) = GetPositions(x, y, z);

            if (chunks[cx, cy, cz] != null)
            {
                chunks[cx, cy, cz].SetBlock(info, bx, by, bz);
            }
            else
            {
                chunks[cx, cy, cz] = new ChunkData();
                chunks[cx, cy, cz].SetBlock(info, bx, by, bz);
            }
        }

        public BlockInfo GetBlock(int x, int y, int z)
        {
            if (!InRegion(x, y, z)) return null;
            var (cx, cy, cz, bx, by, bz) = GetPositions(x, y, z);
            if (chunks[cx, cy, cz] == null) return BlockInfo.AIR;
            return chunks[cx, cy, cz].GetBlock(bx, by, bz);
        }

        public ChunkData GetChunk(int x, int y, int z) => chunks[x, y, z];

        public bool InRegion(int x, int y, int z)
        {
            if (x < this.x * REGION_BLOCK_COUNT || x > (this.x + 1) * REGION_BLOCK_COUNT) return false;
            if (y < this.y * REGION_BLOCK_COUNT || y > (this.y + 1) * REGION_BLOCK_COUNT) return false;
            if (z < this.z * REGION_BLOCK_COUNT || z > (this.z + 1) * REGION_BLOCK_COUNT) return false;
            return true;
        }

        public (int, int, int, int, int, int) GetPositions(int x, int y, int z)
        {
            int rx = x - this.x * REGION_BLOCK_COUNT;
            int ry = y - this.y * REGION_BLOCK_COUNT;
            int rz = z - this.z * REGION_BLOCK_COUNT;

            int cx = rx / ChunkData.CHUNK_SIZE;
            int cy = ry / ChunkData.CHUNK_SIZE;
            int cz = rz / ChunkData.CHUNK_SIZE;

            int bx = rx % ChunkData.CHUNK_SIZE;
            int by = ry % ChunkData.CHUNK_SIZE;
            int bz = rz % ChunkData.CHUNK_SIZE;

            return (cx, cy, cz, bx, by, bz);
        }

        public void Save(string path)
        {
            string fileName = $"r.{x}.{y}.{z}.dat";
            using var file = new FileStream(path + Path.DirectorySeparatorChar + fileName, FileMode.OpenOrCreate);
            using var writer = new BinaryWriter(file);
            for (int x = 0; x < REGION_SIZE; x++)
            {
                for (int y = 0; y < REGION_SIZE; y++)
                {
                    for (int z = 0; z < REGION_SIZE; z++)
                    {
                        if (chunks[x, y, z] != null)
                        {
                            writer.Write(x);
                            writer.Write(y);
                            writer.Write(z);
                            writer.Flush();
                            chunks[x, y, z].Write(writer);
                        }
                    }
                }
            }
        }
    }
}

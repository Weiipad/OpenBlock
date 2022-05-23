using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Chunks
{
    public class Chunk
    {
        public const int CHUNK_SIZE = 16;

        private int[,,] terrain = new int[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        private List<BlockInfo> palette = new List<BlockInfo>();

        public Chunk() { }

        public Chunk(BinaryReader reader)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        terrain[x, y, z] = reader.ReadInt32();
                    }
                }
            }
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                palette.Add(new BlockInfo(reader));
            }
        } 

        public BlockInfo GetBlock(int x, int y, int z)
        {
            return palette[terrain[x, y, z]];
        }

        public void SetBlock(BlockInfo block, int x, int y, int z)
        {
            if (FindPalette(block) is int idx)
            {
                terrain[x, y, z] = idx;
            }
            else
            {
                terrain[x, y, z] = PaletteAdd(block);
            }
        }

        public void Write(BinaryWriter writer)
        {
            for (int x = 0; x < CHUNK_SIZE; x++)
            {
                for (int y = 0; y < CHUNK_SIZE; y++)
                {
                    for (int z = 0; z < CHUNK_SIZE; z++)
                    {
                        writer.Write(terrain[x, y, z]);
                    }
                }
            }
            writer.Flush();
            writer.Write(palette.Count);
            writer.Flush();
            foreach (var binfo in palette)
            {
                binfo.Write(writer);
            }
        }

        private int? FindPalette(BlockInfo info)
        {
            for (int i = 0; i < palette.Count; i++)
            {
                if (BlockInfo.Equals(info, palette[i]))
                {
                    return i;
                }
            }
            return null;
        }

        private int PaletteAdd(BlockInfo info)
        {
            int last = palette.Count;
            palette.Add(info);
            return last;
        }
    }
}

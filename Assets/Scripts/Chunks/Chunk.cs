using OpenBlock.Serialization.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Chunks
{
    public class Chunk : BinaryWritable
    {
        public const int CHUNK_SIZE = 16;

        private int[,,] terrain = new int[CHUNK_SIZE, CHUNK_SIZE, CHUNK_SIZE];
        private BlockPalette blockPalette;

        public Chunk()
        {
            blockPalette = new BlockPalette();
        }

        public Chunk(Stream input)
        {
            using var reader = new BinaryReader(input);
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
            blockPalette = new BlockPalette(reader);
        }

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
            blockPalette = new BlockPalette(reader);
        } 

        public BlockInfo GetBlock(int x, int y, int z)
        {
            return blockPalette[terrain[x, y, z]];
        }

        public void SetBlock(BlockInfo block, int x, int y, int z)
        {
            if (blockPalette.Find(block) is int idx)
            {
                terrain[x, y, z] = idx;
            }
            else
            {
                terrain[x, y, z] = blockPalette.AddBlockInfo(block);
            }
        }

        public string LogPalette()
        {
            return blockPalette.ToString();
        }

        public void WriteToStream(Stream output)
        {
            using var writer = new BinaryWriter(output);
            Write(writer);
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
            blockPalette.Write(writer);
        }
    }
}

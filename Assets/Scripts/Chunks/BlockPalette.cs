using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenBlock.Serialization.Interfaces;

namespace OpenBlock.Chunks
{
    public class BlockPalette : BinaryWritable
    {
        private List<BlockInfo> palette = new List<BlockInfo>();

        public int Count => palette.Count;

        public BlockInfo this[int index]
        {
            get
            {
                if (index >= 0 && index < Count)
                {
                    return palette[index];
                }
                else
                {
                    return null;
                }    
            }
        }

        public BlockPalette()
        {
            palette.Add(BlockInfo.AIR);
        }

        public BlockPalette(Stream stream)
        {
            using var reader = new BinaryReader(stream);
            Init(reader);
        }

        public BlockPalette(BinaryReader reader)
        {
            Init(reader);
        }

        public int? Find(BlockInfo info)
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

        public int AddBlockInfo(BlockInfo info)
        {
            palette.Add(info);
            return palette.Count - 1;
        }

        private void Init(BinaryReader reader)
        {
            var count = reader.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                palette.Add(new BlockInfo(reader));
            }
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(palette.Count);
            writer.Flush();
            foreach (var binfo in palette)
            {
                binfo.Write(writer);
            }
        }

        public override string ToString()
        {
            StringBuilder output = new StringBuilder();
            foreach (var info in palette)
            {
                output.Append($"{info}\n");
            }
            return output.ToString();
        }
    }
}

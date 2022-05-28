using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Terrain.Blocks
{
    public class BlockRegistry
    {
        public static Dictionary<BlockId, IBlock> blocks = new Dictionary<BlockId, IBlock>();

        static BlockRegistry()
        {
            blocks.Add(BlockId.RGBBlock, new RGBBlock());
        }
    }
}

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
            blocks.Add(BlockId.Stone, new CommonBlock(1));
            blocks.Add(BlockId.Grass, new GrassBlock());
            blocks.Add(BlockId.CraftingTable, new CommonBlock(59, 43, 60, 59, 4, 60));
            blocks.Add(BlockId.Furnance, new CommonBlock(45, 62, 44, 45, 62, 45));
            blocks.Add(BlockId.TNT, new CommonBlock(9, 8, 10));
            blocks.Add(BlockId.Log, new PillarBlock(21, 20));
        }
    }
}

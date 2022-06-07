using OpenBlock.Block;
using OpenBlock.Block.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Terrain.BlockModels
{
    public class BlockRegistry
    {
        public static Dictionary<BlockId, IBlockModel> blockModels = new Dictionary<BlockId, IBlockModel>();
        public static Dictionary<BlockId, IBlock> blocks = new Dictionary<BlockId, IBlock>();

        static BlockRegistry()
        {
            blockModels.Add(BlockId.RGBBlock, new RGBBlockModel());
            blockModels.Add(BlockId.Stone, new CommonBlockModel(1));
            blockModels.Add(BlockId.Grass, new GrassBlockModel());
            blockModels.Add(BlockId.CraftingTable, new CommonBlockModel(59, 43, 60, 59, 4, 60));
            blockModels.Add(BlockId.Furnance, new CommonBlockModel(45, 62, 44, 45, 62, 45));
            blockModels.Add(BlockId.TNT, new CommonBlockModel(9, 8, 10));
            blockModels.Add(BlockId.Log, new PillarBlockModel(21, 20));

            blocks.Add(BlockId.RGBBlock, new CommonBlock(0.5f));
            blocks.Add(BlockId.Stone, new CommonBlock(2.0f));
            blocks.Add(BlockId.Grass, new CommonBlock(1.0f));
            blocks.Add(BlockId.CraftingTable, new CommonBlock(1.5f));
            blocks.Add(BlockId.Furnance, new CommonBlock(2.0f));
            blocks.Add(BlockId.TNT, new TNTBlock());
            blocks.Add(BlockId.Log, new CommonBlock(1.5f));
        }
    }
}

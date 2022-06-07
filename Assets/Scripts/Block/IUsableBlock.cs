using OpenBlock.Entity.Player;
using OpenBlock.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Block
{
    public interface IUsableBlock : IBlock
    {
        void OnUse(BlockState state);
    }
}

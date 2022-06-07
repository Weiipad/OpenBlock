using OpenBlock.Core.Event.HUDRequest;
using OpenBlock.Entity.Player;
using OpenBlock.Terrain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Block.Blocks
{
    public class TNTBlock : IUsableBlock
    {
        public float Hardness => 1;

        public void OnUse(BlockState state)
        {
            GameManager.Instance.eventQueue.SendEvent(new OpenDialogEvent("Boom!"));
        }
    }
}

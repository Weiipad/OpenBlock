using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Block.Blocks
{
    public class CommonBlock : IBlock
    {
        private float m_hardness;
        public float Hardness => m_hardness;

        public CommonBlock(float hardness)
        {
            m_hardness = hardness;
        }

    }
}

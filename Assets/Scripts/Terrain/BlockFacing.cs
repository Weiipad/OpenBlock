
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Terrain
{
    [System.Flags]
    public enum BlockFacing: byte
    {
        None = 0,
        West = 1, 
        East = 2, 
        North = 4,
        South = 8, 
        Up = 16, 
        Down = 32,
    }
}

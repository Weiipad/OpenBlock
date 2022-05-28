﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public interface IBlock
    {
        void BuildModel(BlockState state, ref ChunkMeshBuilder builder, Vector3 blockPos, BlockFacing facing);
    }
}

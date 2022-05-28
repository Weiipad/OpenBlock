﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public interface ILevel
    {
        Chunk GetChunk(Vector3Int chunkPos);
        void SaveChunk(Chunk chunk);
        BlockState GetBlock(Vector3Int worldBlockPos);
    }
}

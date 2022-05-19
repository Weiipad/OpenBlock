using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace OpenBlock.Serialization.Interfaces
{
    public interface BinaryWritable
    {
        void Write(BinaryWriter writer);
    }
}
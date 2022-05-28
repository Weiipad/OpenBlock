using OpenBlock.Terrain.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class BlockState: IEquatable<BlockState>
    {
        public static BlockState AIR;
        static BlockState()
        {
            AIR = new BlockState();
            AIR.id = BlockId.Air;
            AIR.properties = null;
        }

        public static BlockState RGB(Color color)
        {
            var ans = new BlockState();
            ans.id = BlockId.RGB;
            ans.properties = new Dictionary<string, string>();
            ans.properties.Add("RGB", color.ToColorCode().ToString());
            return ans;
        }

        public BlockId id;
        public Dictionary<string, string> properties;

        public bool Equals(BlockState other)
        {
            if (other.id != id) return false;
            if (other.properties == null && properties == null) return true;
            if (other.properties.Count != properties.Count) return false;

            foreach (var property in other.properties)
            {
                if (!properties.ContainsKey(property.Key)) return false;
                if (properties[property.Key] != property.Value) return false;
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"{id}\n");

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    builder.Append($"{property.Key}:{property.Value}\n");
                }
            }
            return builder.ToString();
        }
    }
}

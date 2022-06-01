using OpenBlock.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace OpenBlock.Terrain
{
    public class BlockState: IEquatable<BlockState>
    {
        public static BlockState AIR;
        static BlockState()
        {
            AIR = new BlockState(BlockId.Air);
            AIR.properties = null;
        }

        public static BlockState RGB(Color color)
        {
            var ans = new BlockState(BlockId.RGBBlock);
            ans.properties = new Dictionary<string, string>();
            ans.properties.Add("RGB", color.ToColorCode().ToString());
            return ans;
        }

        public BlockId id;
        public Dictionary<string, string> properties;

        public BlockState()
        {

        }

        public BlockState(BlockId id)
        {
            this.id = id;
            properties = null;
        }

        public void AddProperty(string key, string value)
        {
            if (properties == null) properties = new Dictionary<string, string>();
            properties.Add(key, value);
        }

        public bool TryGetProperty(string key, out string value)
        {
            if (properties == null)
            {
                value = null;
                return false;
            }

            return properties.TryGetValue(key, out value);
        }

        public bool Equals(BlockState other)
        {
            if (other.id != id) return false;
            if (other.properties == null && properties == null) return true;
            if (other.properties != null && properties == null) return false;
            if (other.properties == null && properties != null) return false;
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
                    if (property.Key == "RGB")
                    {
                        Color color = uint.Parse(property.Value).ToColor();
                        builder.Append($"{property.Key}:({(int)(color.r * 255)},{(int)(color.g * 255)},{(int)(color.b * 255)})");
                    }
                    else builder.Append($"{property.Key}:{property.Value}\n");
                }
            }
            return builder.ToString();
        }
    }
}

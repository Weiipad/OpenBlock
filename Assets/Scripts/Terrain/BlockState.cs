using OpenBlock.Math;
using OpenBlock.Properties;
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
            ans.properties = new Dictionary<string, Property>();
            ans.properties.Add("RGB", new Property(color.ToColorCode()));
            return ans;
        }

        public BlockId id;
        public Dictionary<string, Property> properties;

        public BlockState()
        {

        }

        public BlockState(BlockId id)
        {
            this.id = id;
            properties = null;
        }

        public void AddProperty(string key, Property value)
        {
            if (properties == null) properties = new Dictionary<string, Property>();
            properties.Add(key, value);
        }

        public bool TryGetProperty(string key, out Property value)
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
                    builder.Append(property.Key);
                    builder.Append(':');
                    if (property.Key == "RGB")
                    {
                        Color color = property.Value.GetUint().ToColor();
                        builder.Append($"({(int)(color.r * 255)},{(int)(color.g * 255)},{(int)(color.b * 255)})");
                    }
                    else if (property.Key == "dir")
                    {
                        builder.Append((Direction)property.Value.GetByte());
                    }
                    else if (property.Key == "axis")
                    {
                        builder.Append((Axis)property.Value.GetByte());
                    }
                    else
                    {
                        builder.Append(property.Value);
                    }
                    builder.Append(Environment.NewLine);
                }
            }
            return builder.ToString();
        }
    }
}

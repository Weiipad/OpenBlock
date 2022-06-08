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
        #region Static Definitions
        public static BlockState AIR;
        static BlockState()
        {
            AIR = new BlockState(BlockId.Air);
        }

        public static BlockState RGB(Color color)
        {
            var ans = new BlockState(BlockId.RGBBlock);
            ans.AddProperty("RGB", new Property(color.ToColorCode()));
            return ans;
        }
        #endregion

        public BlockId id
        {
            get
            {
                return (BlockId)root.GetUint();
            }
        }
        private Property root;

        public BlockState()
        {
            root = new Property((uint)0);
        }

        public BlockState(BlockId id)
        {
            root = new Property((uint)id);
        }

        public void AddProperty(string key, Property value)
        {
            root.AddChild(key, value);
        }

        public bool TryGetProperty(string key, out Property value)
        {
            value = root.GetChild(key);
            return value != null;
        }

        public bool Equals(BlockState other)
        {
            return root == other.root;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(id);
            builder.Append(Environment.NewLine);

            foreach (var child in root.children)
            {
                builder.Append(child.Key);
                builder.Append(':');
                if (child.Key == "RGB")
                {
                    Color color = child.Value.GetUint().ToColor();
                    builder.Append($"({(int)(color.r * 255)},{(int)(color.g * 255)},{(int)(color.b * 255)})");
                }
                else if (child.Key == "axis")
                {
                    builder.Append((Axis)child.Value.GetByte());
                }
                else if (child.Key == "dir")
                {
                    builder.Append((Direction)child.Value.GetByte());
                }
                else
                {
                    builder.Append(child.Value);
                }    
                builder.Append(Environment.NewLine);
            }

            return builder.ToString();
        }
    }
}

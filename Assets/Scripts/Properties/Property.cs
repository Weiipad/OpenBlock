using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace OpenBlock.Properties
{
    public class Property : IEquatable<Property>
    {
        public readonly PropType type;
        private byte[] data;

        private Dictionary<string, Property> m_children;
        public Dictionary<string, Property> children
        {
            get
            {
                if (m_children == null) m_children = new Dictionary<string, Property>();
                return m_children;
            }
        }

        protected Property(PropType type, byte[] data)
        {
            this.type = type;
            this.data = data;
        }

        public Property() : this(PropType.Nil, null) { }

        public Property(bool value) : this(PropType.Bool, BitConverter.GetBytes(value)) { }

        public Property(byte value) : this(PropType.Byte, new byte[] { value }) { }

        public Property(short value) : this(PropType.Short, BitConverter.GetBytes(value)) { }
        public Property(ushort value) : this(PropType.Ushort, BitConverter.GetBytes(value)) { }

        public Property(int value) : this(PropType.Int, BitConverter.GetBytes(value)) { }
        public Property(uint value) : this(PropType.Uint, BitConverter.GetBytes(value)) { }

        public Property(float value) : this(PropType.Float, BitConverter.GetBytes(value)) { }

        public Property(string value) : this(PropType.String, Encoding.UTF8.GetBytes(value)) { }

        public Property(BinaryReader reader)
        {
            type = (PropType)reader.ReadByte();

            ushort dataLen = type switch
            {
                PropType.Bool => sizeof(bool),
                PropType.Byte => sizeof(byte),
                PropType.Ushort => sizeof(ushort),
                PropType.Short => sizeof(short),
                PropType.Uint => sizeof(uint),
                PropType.Int => sizeof(int),
                PropType.Float => sizeof(float),
                PropType.String => reader.ReadUInt16(),
                _ => 0,
            };

            if (dataLen > 0)
            {
                data = reader.ReadBytes(dataLen);
            }

            ushort childCount = reader.ReadUInt16();
            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    ushort tagLen = reader.ReadUInt16();
                    string tag = Encoding.UTF8.GetString(reader.ReadBytes(tagLen));
                    children.Add(tag, new Property(reader));
                }
            }
        }

        #region Getters
        public bool GetBool()
        {
            Assert.IsTrue(PropType.Bool == type);
            return BitConverter.ToBoolean(data, 0);
        }

        public byte GetByte()
        {
            Assert.IsTrue(PropType.Byte == type);
            return data[0];
        }

        public short GetShort()
        {
            Assert.IsTrue(PropType.Short == type);
            return BitConverter.ToInt16(data, 0);
        }

        public ushort GetUshort()
        {
            Assert.IsTrue(PropType.Ushort == type);
            return BitConverter.ToUInt16(data, 0);
        }

        public int GetInt()
        {
            Assert.IsTrue(PropType.Int == type);
            return BitConverter.ToInt32(data, 0);
        }

        public uint GetUint()
        {
            Assert.IsTrue(PropType.Uint == type);
            return BitConverter.ToUInt32(data, 0);
        }

        public float GetFloat()
        {
            Assert.IsTrue(PropType.Float == type);
            return BitConverter.ToSingle(data, 0);
        }

        public string GetString()
        {
            Assert.IsTrue(PropType.String == type);
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public Property GetChild(string tag)
        {
            if (m_children == null || m_children.Count == 0) return null;
            if (children.TryGetValue(tag, out Property p))
            {
                return p;
            }
            return null;
        }
        #endregion

        #region Setters
        public void Set(uint value)
        {
            Assert.IsTrue(PropType.Uint == type);
            Array.Copy(BitConverter.GetBytes(value), 0, data, 0, sizeof(uint));
        }

        public void Set(int value)
        {
            Assert.IsTrue(PropType.Int == type);
            Array.Copy(BitConverter.GetBytes(value), 0, data, 0, sizeof(int));
        }

        public void Set(float value)
        {
            Assert.IsTrue(PropType.Float == type);
            Array.Copy(BitConverter.GetBytes(value), 0, data, 0, sizeof(float));
        }

        public void AddChild(string tag, Property child)
        {
            children.Add(tag, child);
        }
        #endregion
        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)type);
            if (type == PropType.String) writer.Write((ushort)data.Length);
            if (data != null && data.Length != 0) writer.Write(data, 0, data.Length);
            if (m_children == null)
            {
                writer.Write((ushort)0);
                return;
            }
            writer.Write((ushort)children.Count);
            foreach (var child in children)
            {
                writer.Write((ushort)child.Key.Length);
                writer.Write(Encoding.UTF8.GetBytes(child.Key));
                child.Value.Write(writer);
            }
        }

        

        public static ushort? GetDataLen(PropType type)
        {
            return type switch
            {
                PropType.Bool => sizeof(bool),
                PropType.Byte => sizeof(byte),
                PropType.Ushort => sizeof(ushort),
                PropType.Short => sizeof(short),
                PropType.Uint => sizeof(uint),
                PropType.Int => sizeof(int),
                PropType.Float => sizeof(float),
                PropType.String => null,
                _ => 0,
            };
        }

        public override string ToString()
        {
            string ctx = type switch
            {
                PropType.Byte => GetByte().ToString(),
                PropType.Bool => GetBool().ToString(),
                PropType.Short => GetShort().ToString(),
                PropType.Ushort => GetUshort().ToString(),
                PropType.Uint => GetUint().ToString(),
                PropType.Int => GetInt().ToString(),
                PropType.Float => GetFloat().ToString(),
                PropType.String => GetString(),
                _ => "Nil",
            };

            if (m_children == null) return ctx;

            StringBuilder builder = new StringBuilder();
            builder.Append(ctx);
            builder.Append('{');
            foreach (var child in children)
            {
                builder.Append(child.Key);
                builder.Append(':');
                builder.Append(child.Value);
                builder.Append(',');
            }
            if (children.Count != 0) builder.Remove(builder.Length - 1, 1);
            builder.Append('}');
            return builder.ToString();
        }

        public bool Equals(Property other)
        {
            if (other.type != type) return false;
            if (other.data.Length != data.Length) return false;
            for (int i = 0; i < data.Length; i++)
            {
                if (other.data[i] != data[i]) return false;
            }
            if ((m_children == null) ^ (other.m_children == null)) return false;
            if (m_children != null)
            {
                if (other.m_children.Count != m_children.Count) return false;
                foreach (var child in m_children)
                {
                    if (!other.m_children.ContainsKey(child.Key)) return false;
                    if (other.m_children[child.Key] != child.Value) return false;
                }
            }
            return true;
        }
    }
}

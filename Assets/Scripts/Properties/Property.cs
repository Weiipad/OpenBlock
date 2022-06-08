using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Assertions;

namespace OpenBlock.Properties
{
    public class Property
    {
        public readonly PropType type;
        private byte[] data;

        

        private Property(PropType type, byte[] data)
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
        }
        
        public bool GetBool()
        {
            Assert.AreEqual(PropType.Bool, type);
            return BitConverter.ToBoolean(data, 0);
        }

        public byte GetByte()
        {
            Assert.AreEqual(PropType.Byte, type);
            return data[0];
        }

        public short GetShort()
        {
            Assert.AreEqual(PropType.Short, type);
            return BitConverter.ToInt16(data, 0);
        }

        public ushort GetUshort()
        {
            Assert.AreEqual(PropType.Ushort, type);
            return BitConverter.ToUInt16(data, 0);
        }

        public int GetInt()
        {
            Assert.AreEqual(PropType.Int, type);
            return BitConverter.ToInt32(data, 0);
        }

        public uint GetUint()
        {
            Assert.AreEqual(PropType.Uint, type);
            return BitConverter.ToUInt32(data, 0);
        }

        public float GetFloat()
        {
            Assert.AreEqual(PropType.Float, type);
            return BitConverter.ToSingle(data, 0);
        }

        public string GetString()
        {
            Assert.AreEqual(PropType.String, type);
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)type);
            if (type == PropType.String) writer.Write((ushort)data.Length);
            if (data != null) writer.Write(data);
        }

        public override string ToString()
        {
            return type switch
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
        }
    }
}

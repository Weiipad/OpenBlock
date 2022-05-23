using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Chunks
{
    public struct Attribute
    {
        public enum AttribType : byte
        {
            Int, Float, Bool, Axis, Direction, String
        }

        public AttribType type;
        public byte[] value;

        #region Direct Constructors
        public Attribute(int value)
        {
            type = AttribType.Int;
            this.value = BitConverter.GetBytes(value);
        }

        public Attribute(float value)
        {
            type = AttribType.Float;
            this.value = BitConverter.GetBytes(value);
        }

        public Attribute(bool value)
        {
            type = AttribType.Bool;
            this.value = BitConverter.GetBytes(value);
        }

        public Attribute(string value)
        {
            type = AttribType.String;
            this.value = Encoding.UTF8.GetBytes(value);
        }
        public Attribute(Axis value)
        {
            type = AttribType.Axis;
            this.value = new byte[] { (byte)value };
        }

        public Attribute(Direction value) : this (AttribType.Direction, new byte[] { (byte)value }) { }

        public Attribute(AttribType ty, byte[] value)
        {
            type = ty;
            this.value = value;
        }
        #endregion

        /*
         * --------------------------------------------------------
         * |   0x01    | nameLen |   0x01   |   0x01   | valueLen |
         * --------------------------------------------------------
         * |  nameLen  |  name   |   type   | valueLen |  value   |
         * --------------------------------------------------------
         */
        public Attribute(BinaryReader reader)
        {
            type = (AttribType)reader.ReadByte();
            var valueLen = reader.ReadByte();
            value = reader.ReadBytes(valueLen);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write((byte)type);
            writer.Write((byte)value.Length);
            writer.Write(value);
            writer.Flush();
        }

        public void WriteToStream(Stream stream)
        {
            using var writer = new BinaryWriter(stream);
            Write(writer);
        }


        #region AsValues
        public int AsInt()
        {
            if (type != AttribType.Int) throw new AttribTypeMismatchException(type);
            return BitConverter.ToInt32(value, 0);
        }

        public float AsFloat()
        {
            if (type != AttribType.Float) throw new AttribTypeMismatchException(type);
            return BitConverter.ToSingle(value, 0);
        }

        public bool AsBool()
        {
            if (type != AttribType.Bool) throw new AttribTypeMismatchException(type);
            return BitConverter.ToBoolean(value, 0);
        }

        public string AsString()
        {
            if (type != AttribType.String) throw new AttribTypeMismatchException(type);
            return Encoding.UTF8.GetString(value, 0, value.Length);
        }

        public Axis AsAxis()
        {
            if (type != AttribType.Axis) throw new AttribTypeMismatchException(type);
            return (Axis)value[0];
        }

        public Direction AsDir()
        {
            if (type != AttribType.Direction) throw new AttribTypeMismatchException(type);
            return (Direction)value[0];
        }
        #endregion

        public static bool Equals(Attribute a, Attribute b)
        {
            if (a.type != b.type) return false;

            int len = a.value.Length;
            if (b.value.Length < len) len = b.value.Length;

            for (int i = 0; i < len; i++)
            {
                if (a.value[i] != b.value[i]) return false;
            }

            return true;
        }
    }
}

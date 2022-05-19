using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Chunks
{
    public enum Direction: byte
    {
        Up, Down, South, North, East, West
    }

    public enum Axis: byte
    {
        X, Y, Z
    }

    public static class TypeConverts
    {
        public static byte[] GetBytes(Axis axis)
        {
            return new byte[] { (byte)axis };
        }

        public static byte[] GetBytes(Direction direction)
        {
            return new byte[] { (byte)direction };
        }
    }

    public class AttribTypeMismatchException: Exception
    {
        public AttribTypeMismatchException() : base() { }
        public AttribTypeMismatchException(string msg) : base(msg) { }
        public AttribTypeMismatchException(Attribute.AttribType type) : base($"The attribute is {type}") { }
    }

}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBlock.Chunks
{
    public class BlockInfo
    {
        public int id;
        public Dictionary<string, Attribute> attributes = new Dictionary<string, Attribute>();

        public Attribute? this[string name] => GetAttribute(name);

        public static BlockInfo AIR;

        static BlockInfo()
        {
            AIR = new BlockInfo(0);
        }
        public BlockInfo(int id)
        {
            this.id = id;
        }

        public BlockInfo(BinaryReader reader)
        {
            id = reader.ReadInt32();
            var countAttrs = reader.ReadByte();
            for (int i = 0; i < countAttrs; i++)
            {
                var nameLen = reader.ReadByte();
                string name = Encoding.UTF8.GetString(reader.ReadBytes(nameLen));
                attributes.Add(name, new Attribute(reader));
            }
        }

        public Attribute? GetAttribute(string name)
        {
            if (attributes.TryGetValue(name, out Attribute attr))
            {
                return attr;
            } 
            else
            {
                return null;
            }
        }

        #region Set Attribute
        public void SetAttribute(string name, bool value)
        {
            if (GetAttribute(name) is Attribute attr)
            {
                if (attr.type == Attribute.AttribType.Bool)
                {
                    attr.value = BitConverter.GetBytes(value);
                }
                else
                {
                    throw new Exception($"Attribute type error: {name}({attr.type})");
                }
            }
            throw new Exception($"Unable to find attribute: {name}");
        }

        public void SetAttribute(string name, int value)
        {
            if (GetAttribute(name) is Attribute attr)
            {
                if (attr.type == Attribute.AttribType.Int)
                {
                    attr.value = BitConverter.GetBytes(value);
                }
                else
                {
                    throw new Exception($"Attribute type error: {name}({attr.type})");
                }
            }
            throw new Exception($"Unable to find attribute: {name}");
        }

        public void SetAttribute(string name, float value)
        {
            if (GetAttribute(name) is Attribute attr)
            {
                if (attr.type == Attribute.AttribType.Float)
                {
                    attr.value = BitConverter.GetBytes(value);
                }
                else
                {
                    throw new Exception($"Attribute type error: {name}({attr.type})");
                }
            }
            throw new Exception($"Unable to find attribute: {name}");
        }

        public void SetAttribute(string name, string value)
        {
            if (GetAttribute(name) is Attribute attr)
            {
                if (attr.type == Attribute.AttribType.Int)
                {
                    attr.value = Encoding.UTF8.GetBytes(value);
                }
                else
                {
                    throw new Exception($"Attribute type error: {name}({attr.type})");
                }
            }
            throw new Exception($"Unable to find attribute: {name}");
        }

        public void SetAttribute(string name, Axis axis)
        {
            if (GetAttribute(name) is Attribute attr)
            {
                if (attr.type == Attribute.AttribType.Axis)
                {
                    attr.value = TypeConverts.GetBytes(axis);
                }
                else
                {
                    throw new Exception($"Attribute type error: {name}({attr.type})");
                }
            }
            throw new Exception($"Unable to find attribute: {name}");
        }

        public void SetAttribute(string name, Direction direction)
        {
            if (GetAttribute(name) is Attribute attr)
            {
                if (attr.type == Attribute.AttribType.Direction)
                {
                    attr.value = TypeConverts.GetBytes(direction);
                }
                else
                {
                    throw new Exception($"Attribute type error: {name}({attr.type})");
                }
            }
            throw new Exception($"Unable to find attribute: {name}");
        }
        #endregion

        #region Add Attribute
        public void AddAttribute(string name, int value)
        {
            attributes.Add(name, new Attribute(value));
        }
        public void AddAttribute(string name, float value)
        {
            attributes.Add(name, new Attribute(value));
        }
        public void AddAttribute(string name, bool value)
        {
            attributes.Add(name, new Attribute(value));
        }
        public void AddAttribute(string name, string value)
        {
            attributes.Add(name, new Attribute(value));
        }
        public void AddAttribute(string name, Axis value)
        {
            attributes.Add(name, new Attribute(value));
        }

        public void AddAttribute(string name, Direction value)
        {
            attributes.Add(name, new Attribute(value));
        }
        #endregion

        public void WriteToStream(Stream stream)
        {
            using var writer = new BinaryWriter(stream);
            Write(writer);
        }

        public void Write(BinaryWriter writer)
        {
            writer.Write(id);
            writer.Write((byte)attributes.Count);
            foreach (var pair in attributes)
            {
                var name = pair.Key;
                var attr = pair.Value;

                writer.Write((byte)name.Length);
                writer.Write(Encoding.UTF8.GetBytes(name));
                attr.Write(writer);
            }
            writer.Flush();
        }

        public byte[] ToBytes()
        {
            MemoryStream output = new MemoryStream();
            WriteToStream(output);
            return output.ToArray();
        }

        public static bool Equals(BlockInfo a, BlockInfo b)
        {
            if (a.attributes.Count != b.attributes.Count) return false;

            foreach (var pair in a.attributes)
            {
                var name = pair.Key;
                var attr = pair.Value;
                if (b[name] is Attribute bAttr)
                {
                    if (!Attribute.Equals(attr, bAttr)) return false;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.Append($"{id}:{{");
            foreach (var pair in attributes)
            {
                var name = pair.Key;
                var attr = pair.Value;
                switch (attr.type)
                {
                    case Attribute.AttribType.Int:
                        sb.Append($"\"{name}\":{attr.AsInt()},");
                        break;
                    case Attribute.AttribType.Float:
                        sb.Append($"\"{name}\":{attr.AsFloat()},");
                        break;
                    case Attribute.AttribType.Bool:
                        sb.Append($"\"{name}\":{attr.AsBool()},");
                        break;
                    case Attribute.AttribType.String:
                        sb.Append($"\"{name}\":{attr.AsString()},");
                        break;
                    case Attribute.AttribType.Axis:
                        sb.Append($"\"{name}\":{attr.AsAxis()},");
                        break;
                    case Attribute.AttribType.Direction:
                        sb.Append($"\"{name}\":{attr.AsDir()},");
                        break;
                }
            }
            if (attributes.Count != 0) sb.Remove(sb.Length - 1, 1);
            sb.Append("}");
            return sb.ToString();
        }
    }
}

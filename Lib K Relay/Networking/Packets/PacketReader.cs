using System;
using System.IO;
using System.Net;
using System.Text;

namespace Lib_K_Relay.Networking.Packets
{
    public class PacketReader : BinaryReader
    {
        public PacketReader(MemoryStream input)
            : base(input, Encoding.UTF8)
        {
        }

        public override short ReadInt16()
        {
            return IPAddress.NetworkToHostOrder(base.ReadInt16());
        }

        public override ushort ReadUInt16()
        {
            return (ushort)IPAddress.NetworkToHostOrder((short)base.ReadUInt16());
        }

        public override int ReadInt32()
        {
            return IPAddress.NetworkToHostOrder(base.ReadInt32());
        }

        public int ReadCompressedInt()
        {
            byte firstByte = this.ReadByte();
            bool isNegative = ((uint)firstByte & 64U) > 0U;
            byte shift = 6; 
            int result = (int)firstByte & 63; 

            while (((int)firstByte & 128) != 0)
            {
                firstByte = this.ReadByte();
                result |= ((int)firstByte & (int)sbyte.MaxValue) << (int)shift; 
                shift += 7; 
            }

            if (isNegative)
                result = -result;

            return result;
        }

        public override float ReadSingle()
        {
            var arr = base.ReadBytes(4);
            Array.Reverse(arr);
            return BitConverter.ToSingle(arr, 0);
        }

        public override string ReadString()
        {
            return Encoding.UTF8.GetString(ReadBytes(ReadInt16()));
        }

        public string ReadUtf32()
        {
            return Encoding.UTF8.GetString(ReadBytes(ReadInt32()));
        }
    }
}
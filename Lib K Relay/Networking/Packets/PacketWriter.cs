using System;
using System.IO;
using System.Net;
using System.Text;

namespace Lib_K_Relay.Networking.Packets
{
    public class PacketWriter : BinaryWriter
    {
        public PacketWriter(MemoryStream input)
            : base(input)
        {
        }

        public override void Write(short value)
        {
            base.Write(IPAddress.NetworkToHostOrder(value));
        }

        public override void Write(ushort value)
        {
            base.Write((ushort)IPAddress.HostToNetworkOrder((short)value));
        }

        public override void Write(int value)
        {
            base.Write(IPAddress.NetworkToHostOrder(value));
        }

        public override void Write(float value)
        {
            var b = BitConverter.GetBytes(value);
            Array.Reverse(b);
            base.Write(b);
        }

        public override void Write(string value)
        {
            var data = Encoding.UTF8.GetBytes(value);
            Write((short)data.Length);
            base.Write(data);
        }

        public void WriteCompressedInt(int value)
        {
            int num1 = value < 0 ? 1 : 0;
            uint num2 = num1 != 0 ? (uint)-value : (uint)value;
            byte num3 = (byte)(num2 & 63U);
            if (num1 != 0)
                num3 |= (byte)64;
            uint num4 = num2 >> 6;
            bool flag = num4 > 0U;
            if (flag)
                num3 |= (byte)128;
            this.Write(num3);
            while (flag)
            {
                byte num5 = (byte)(num4 & (uint)sbyte.MaxValue);
                num4 >>= 7;
                flag = num4 > 0U;
                if (flag)
                    num5 |= (byte)128;
                this.Write(num5);
            }
        }

        public void WriteUtf32(string value)
        {
            Write(value.Length);
            Write(Encoding.UTF8.GetBytes(value));
        }

        public static void BlockCopyInt32(byte[] data, int int32)
        {
            var lengthBytes = BitConverter.GetBytes(IPAddress.NetworkToHostOrder(int32));
            data[0] = lengthBytes[0];
            data[1] = lengthBytes[1];
            data[2] = lengthBytes[2];
            data[3] = lengthBytes[3];
        }
    }
}
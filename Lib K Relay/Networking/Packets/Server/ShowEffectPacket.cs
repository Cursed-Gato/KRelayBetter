using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;
using Lib_K_Relay.Utilities;
using System.IO;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ShowEffectPacket : Packet
    {
        private const int EffectBitColor = 1;
        private const int EffectBitPos1X = 2;
        private const int EffectBitPos1Y = 4;
        private const int EffectBitPos2X = 8;
        private const int EffectBitPos2Y = 16;
        private const int EffectBitPos1 = 6;
        private const int EffectBitPos2 = 24;
        private const int EffectBitDuration = 32;
        private const int EffectBitId = 64;
        private const int UnknownBitId = 128;
        private static readonly Argb EmptyArgb = new Argb(uint.MaxValue);
        public byte EffectValue;
        public EffectType EffectType;
        public int TargetId;
        public Location PosA = Location.Empty;
        public Location PosB = Location.Empty;
        public Argb Color;
        public float Duration;
        public byte UnknownValue;

        public override PacketType Type => PacketType.SHOWEFFECT;

        public override void Read(PacketReader r)
        {
            EffectValue = r.ReadByte();
            EffectType = (EffectType)EffectValue;
            byte num = r.ReadByte();
            TargetId = (num & 64) != 0 ? r.ReadCompressedInt() : 0;
            PosA.X = (num & 2) != 0 ? r.ReadSingle() : 0.0f;
            PosA.Y = (num & 4) != 0 ? r.ReadSingle() : 0.0f;
            PosB.X = (num & 8) != 0 ? r.ReadSingle() : 0.0f;
            PosB.Y = (num & 16) != 0 ? r.ReadSingle() : 0.0f;
            Color = (num & 1) != 0 ? Argb.Read(r) : ShowEffectPacket.EmptyArgb;
            Duration = (num & 32) != 0 ? r.ReadSingle() : 1f;
            UnknownValue = (num & 128) != 0 ? r.ReadByte() : (byte)100;
        }

        public override void Write(PacketWriter w)
        {
            using (MemoryStream input = new MemoryStream())
            {
                using (PacketWriter w1 = new PacketWriter(input))
                {
                    byte num = 0;
                    if (TargetId != 0)
                    {
                        num |= 64;
                        w1.WriteCompressedInt(TargetId);
                    }
                    if (PosA.X != 0.0)
                    {
                        num |= 2;
                        w1.Write(PosA.X);
                    }
                    if (PosA.Y != 0.0)
                    {
                        num |= 4;
                        w1.Write(PosA.Y);
                    }
                    if (PosB.X != 0.0)
                    {
                        num |= 8;
                        w1.Write(PosB.X);
                    }
                    if (PosB.Y != 0.0)
                    {
                        num |= 16;
                        w1.Write(PosB.Y);
                    }
                    if (Color.A != ShowEffectPacket.EmptyArgb.A ||
                        Color.R != ShowEffectPacket.EmptyArgb.R ||
                        Color.G != ShowEffectPacket.EmptyArgb.G ||
                        Color.B != ShowEffectPacket.EmptyArgb.B)
                    {
                        num |= 1;
                        Color.Write(w1);
                    }
                    if (Duration != 1.0)
                    {
                        num |= 32;
                        w1.Write(Duration);
                    }
                    if (UnknownValue != 100)
                    {
                        num |= 128;
                        w1.Write(UnknownValue);
                    }
                    w.Write(EffectValue);
                    w.Write(num);
                    input.WriteTo(w.BaseStream);
                }
            }
        }
    }
}
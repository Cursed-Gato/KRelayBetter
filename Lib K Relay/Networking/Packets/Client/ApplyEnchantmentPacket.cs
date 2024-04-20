
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ApplyEnchantmentPacket : Packet
    {
        public short UNKOWN1;
        public short EnchantmentType;
        public bool Add;
        public byte EnchantmentSlotIdx;

        public override PacketType Type => PacketType.APPLYENCHANTMENT;

        public override void Read(PacketReader r)
        {
            UNKOWN1 = r.ReadInt16();
            EnchantmentType = r.ReadInt16();
            Add = r.ReadBoolean();
            EnchantmentSlotIdx = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(UNKOWN1);
            w.Write(EnchantmentType);
            w.Write(Add);
            w.Write(EnchantmentSlotIdx);
        }
    }
}


namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UnlockEnchantmentSlotPacket : Packet
    {
        public short EnchantmentId;
        public byte SlotIdx;

        public override PacketType Type => PacketType.UNLOCKENCHANTMENTSLOT;

        public override void Read(PacketReader r)
        {
            EnchantmentId = r.ReadInt16();
            SlotIdx = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(EnchantmentId);
            w.Write(SlotIdx);
        }
    }
}

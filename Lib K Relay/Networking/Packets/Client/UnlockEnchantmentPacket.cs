
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UnlockEnchantmentPacket : Packet
    {
        public short UNKNWON;
        public short EnchantmentType;

        public override PacketType Type => PacketType.UNLOCKENCHANTMENT;

        public override void Read(PacketReader r)
        {
            UNKNWON = r.ReadInt16();
            EnchantmentType = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(UNKNWON);
            w.Write(EnchantmentType);
        }
    }
}

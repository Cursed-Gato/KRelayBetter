
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class RerollAllEnchantmentsPacket : Packet
    {
        public short EnchantmentId;

        public override PacketType Type => PacketType.REROLLENCHANTMENTS;

        public override void Read(PacketReader r)
        {
            EnchantmentId = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(EnchantmentId);
        }
    }
}

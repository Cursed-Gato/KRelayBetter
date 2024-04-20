
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ResetEnchantmentRerollCountPacket : Packet
    {
        public short EnchanmentId;

        public override PacketType Type => PacketType.RESETENCHANTMENTSREROLLCOUNT;

        public override void Read(PacketReader r)
        {
            EnchanmentId = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(EnchanmentId);
        }
    }
}

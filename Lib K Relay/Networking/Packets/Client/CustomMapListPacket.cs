
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CustomMapListPacket : Packet
    {
        public override PacketType Type => PacketType.CUSTOMMAPLIST;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}

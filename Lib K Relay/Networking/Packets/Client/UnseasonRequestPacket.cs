
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UnseasonRequestPacket : Packet
    {
        public override PacketType Type => PacketType.UNSEASONREQUEST;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}

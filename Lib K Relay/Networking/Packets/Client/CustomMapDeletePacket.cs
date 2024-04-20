
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CustomMapDeletePacket : Packet
    {
        public int GameId;

        public override PacketType Type => PacketType.CUSTOMMAPDELETE;

        public override void Read(PacketReader r)
        {
            GameId = r.ReadCompressedInt();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(GameId);
        }
    }
}

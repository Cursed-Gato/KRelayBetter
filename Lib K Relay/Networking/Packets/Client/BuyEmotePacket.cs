
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class BuyEmotePacket : Packet
    {
        public int EmoteType;

        public override PacketType Type => PacketType.BUYEMOTE;

        public override void Read(PacketReader r)
        {
            EmoteType = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(EmoteType);
        }
    }
}

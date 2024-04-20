
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class SetTrackedSeasonPacket : Packet
    {
        public int SeasonId;

        public override PacketType Type => PacketType.SETTRACKEDSEASON;

        public override void Read(PacketReader r)
        {
            SeasonId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(SeasonId);
        }
    }
}

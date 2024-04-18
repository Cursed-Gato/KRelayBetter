namespace Lib_K_Relay.Networking.Packets.Server
{
    public class RealmScoreUpdatePacket : Packet
    {
        public int Score;

        public override PacketType Type => PacketType.REALMSCOREUPDATE;

        public override void Read(PacketReader r)
        {
            Score = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Score);
        }
    }
}

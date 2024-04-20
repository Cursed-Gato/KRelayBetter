
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class SetGraveStonePacket : Packet
    {
        public int GraveStoneType;
        public int Tier;

        public override PacketType Type => PacketType.SETGRAVESTONE;

        public override void Read(PacketReader r)
        {
            GraveStoneType = r.ReadInt32();
            Tier = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(GraveStoneType);
            w.Write(Tier);
        }
    }
}


namespace Lib_K_Relay.Networking.Packets.Client
{
    public class BoostBPMilestonePacket : Packet
    {
        public byte MilestoneCount;

        public override PacketType Type => PacketType.BOOSTBPMILESTONE;

        public override void Read(PacketReader r)
        {
            MilestoneCount = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(MilestoneCount);
        }
    }
}


namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ClaimMissionPacket : Packet
    {
        public int SeasonId;
        public byte MissionPositionalIdx;
        public byte RequestId;
        public ushort Mask;

        public override PacketType Type => PacketType.CLAIMMISSION;

        public override void Read(PacketReader r)
        {
            SeasonId = r.ReadInt32();
            MissionPositionalIdx = r.ReadByte();
            RequestId = r.ReadByte();
            Mask = r.ReadUInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(SeasonId);
            w.Write(MissionPositionalIdx);
            w.Write(RequestId);
            w.Write(Mask);
        }
    }
}


namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PartyJoinRequestPacket : Packet
    {
        public uint PartyId;
        public byte unkown1;

        public override PacketType Type => PacketType.PARTYJOINREQUEST;

        public override void Read(PacketReader r)
        {
            PartyId = r.ReadUInt32();
            unkown1 = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PartyId);
            w.Write(unkown1);
        }
    }
}

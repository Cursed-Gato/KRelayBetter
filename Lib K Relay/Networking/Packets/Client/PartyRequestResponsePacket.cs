
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PartyInvitationResponsePacket : Packet
    {
        public uint PartyId;
        public byte AcceptInvite;

        public override PacketType Type => PacketType.PARTYINVITATIONRESPONSE;

        public override void Read(PacketReader r)
        {
            PartyId = r.ReadUInt32();
            AcceptInvite = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PartyId);
            w.Write(AcceptInvite);
        }
    }
}
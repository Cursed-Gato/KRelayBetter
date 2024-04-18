namespace Lib_K_Relay.Networking.Packets.Client
{
    public class IncomingPartyInvitationPacket : Packet
    {
        uint PartyId;
        string InviterName;
        public override PacketType Type => PacketType.INCOMINGPARTINVITATION;

        public override void Read(PacketReader r)
        {
            PartyId = r.ReadUInt32();
            InviterName = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PartyId);
            w.Write(InviterName);
        }
    }
}

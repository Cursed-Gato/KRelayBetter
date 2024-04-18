namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PartyActionPacket : Packet
    {
        ushort playerId;
        PartyActionId actionId;

        public override PacketType Type => PacketType.PARTYACTION;

        public override void Read(PacketReader r)
        {
            playerId = r.ReadUInt16();
            actionId = (PartyActionId)r.ReadByte();

        }

        public override void Write(PacketWriter w)
        {
            w.Write(playerId);
            w.Write((byte)actionId);

        }
        enum PartyActionId
        {
            None = 0,
            Failed = 1,
            Kicked = 2,
            KickNotFound = 3,
            PromotedToLeader = 4,
            PromoteNotFound = 5,
            LeftParty = 6
        }
    }
}

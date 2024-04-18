namespace Lib_K_Relay.Networking.Packets.Client
{
    public class QuestFetchAskPacket : Packet
    {
        public override PacketType Type => PacketType.QUESTFETCHASK;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}
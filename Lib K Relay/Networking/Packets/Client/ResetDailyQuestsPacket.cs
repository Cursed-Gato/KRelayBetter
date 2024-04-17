namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ResetDailyQuestsPacket : Packet
    {
        public override PacketType Type => PacketType.RESETDAILYQUESTS;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}
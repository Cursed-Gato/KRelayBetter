namespace Lib_K_Relay.Networking.Packets.Server
{
    public class QuestRedeemResponsePacket : Packet
    {
        public string Message;
        public bool Success;

        public override PacketType Type => PacketType.QUESTREDEEMRESPONSE;

        public override void Read(PacketReader r)
        {
            Success = r.ReadBoolean();
            Message = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Success);
            w.Write(Message);
        }
    }
}
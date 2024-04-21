namespace Lib_K_Relay.Networking.Packets.Server
{
    public class BuyResultPacket : Packet
    {
        public string Message;
        public int Result;

        public override PacketType Type => PacketType.BUYRESULT;

        public override void Read(PacketReader r)
        {
            Result = r.ReadInt32();
            Message = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Result);
            w.Write(Message);
        }
    }
}
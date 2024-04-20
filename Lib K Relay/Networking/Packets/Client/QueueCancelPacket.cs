namespace Lib_K_Relay.Networking.Packets.Client
{
    public class QueueCancelPacket : Packet
    {
        public string Guid;

        public override PacketType Type => PacketType.QUEUECANCEL;

        public override void Read(PacketReader r)
        {
            Guid = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Guid);
        }
    }
}
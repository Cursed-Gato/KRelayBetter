namespace Lib_K_Relay.Networking.Packets.Server
{
    public class QueueInformationPacket : Packet
    {
        public ushort CurrentPosition;
        public ushort MaxPosition;

        public override PacketType Type => PacketType.QUEUE;

        public override void Read(PacketReader r)
        {
            CurrentPosition = r.ReadUInt16();
            MaxPosition = r.ReadUInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CurrentPosition);
            w.Write(MaxPosition);
        }
    }
}
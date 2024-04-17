namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ShootAckPacket : Packet
    {
        public int Time;
        public short Count;

        public override PacketType Type => PacketType.SHOOTACKCOUNTER;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            Count = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(Count);
        }
    }
}
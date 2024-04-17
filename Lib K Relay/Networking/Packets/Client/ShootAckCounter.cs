namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ShootAckCounter : Packet
    {
        public short Amount;
        public int Time;

        public override PacketType Type => PacketType.SHOOTACKCOUNTER;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            Amount = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(Amount);
        }
    }
}
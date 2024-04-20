
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class SetAbilityPacket : Packet
    {
        public int Time;
        public sbyte Index;
        public override PacketType Type => PacketType.SETABILITY;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            Index = r.ReadSByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(Index);
        }
    }
}

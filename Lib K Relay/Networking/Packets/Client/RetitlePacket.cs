
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class RetitlePacket : Packet
    {
        public int Prefix;
        public int Suffix;

        public override PacketType Type => PacketType.RETITLE;

        public override void Read(PacketReader r)
        {
            Prefix = r.ReadInt32();
            Suffix = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Prefix);
            w.Write(Suffix);
        }
    }
}

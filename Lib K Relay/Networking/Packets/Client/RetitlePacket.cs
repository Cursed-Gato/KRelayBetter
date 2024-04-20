
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class RetitlePacket : Packet
    {
        public int PreFix;
        public int Suffix;

        public override PacketType Type => PacketType.RETITLE;

        public override void Read(PacketReader r)
        {
            PreFix = r.ReadInt32();
            Suffix = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PreFix);
            w.Write(Suffix);
        }
    }
}

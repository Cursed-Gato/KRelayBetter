
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class SetDiscoverablePacket : Packet
    {
        public bool IsDiscoverable;
        public short Icon;

        public override PacketType Type => PacketType.SETDISCOVERABLE;

        public override void Read(PacketReader r)
        {
            IsDiscoverable = r.ReadBoolean();
            Icon = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(IsDiscoverable);
            w.Write(Icon);
        }
    }
}

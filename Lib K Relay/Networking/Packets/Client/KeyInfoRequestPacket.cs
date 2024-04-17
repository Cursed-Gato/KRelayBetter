namespace Lib_K_Relay.Networking.Packets.Client
{
    public class KeyInfoRequestPacket : Packet
    {
        public int ItemType;

        public override PacketType Type => PacketType.KEYINFOREQUEST;

        public override void Read(PacketReader r)
        {
            ItemType = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ItemType);
        }
    }
}
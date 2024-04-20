
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PlayerCalloutPacket : Packet
    {
        public const byte PortalDrop = 1;

        private byte CaloutType;
        private int ObjectId;

        public override PacketType Type => PacketType.PLAYERCALLOUT;

        public override void Read(PacketReader r)
        {
            CaloutType = r.ReadByte();
            ObjectId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CaloutType);
            w.Write(ObjectId);
        }
    }
}

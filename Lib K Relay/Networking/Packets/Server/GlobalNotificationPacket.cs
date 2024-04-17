namespace Lib_K_Relay.Networking.Packets.Server
{
    public class GlobalNotificationPacket : Packet
    {
        public string Text;
        public int TypeId;

        public override PacketType Type => PacketType.NOTIFICATION;

        public override void Read(PacketReader r)
        {
            TypeId = r.ReadInt32();
            Text = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(TypeId);
            w.Write(Text);
        }
    }
}
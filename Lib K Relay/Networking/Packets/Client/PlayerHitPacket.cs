namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PlayerHitPacket : Packet
    {
        public short BulletId;
        public int ObjectId;

        public override PacketType Type => PacketType.PLAYERHIT;

        public override void Read(PacketReader r)
        {
            BulletId = r.ReadInt16();
            ObjectId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(BulletId);
            w.Write(ObjectId);
        }
    }
}
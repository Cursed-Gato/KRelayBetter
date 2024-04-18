namespace Lib_K_Relay.Networking.Packets.Client
{
    public class EnemyHitPacket : Packet
    {
        public short BulletId;
        public bool Killed;
        public int OwnerId;
        public int PlayerId;
        public int TargetId;
        public int Time;

        public override PacketType Type => PacketType.ENEMYHIT;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            BulletId = r.ReadInt16();
            PlayerId = r.ReadInt32();
            TargetId = r.ReadInt32();
            Killed = r.ReadBoolean();
            // proj owner's id, player if it's a normal playershoot and the summoner thing if the shot is theirs
            OwnerId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(BulletId);
            w.Write(PlayerId);
            w.Write(TargetId);
            w.Write(Killed);
            w.Write(OwnerId);
        }
    }
}
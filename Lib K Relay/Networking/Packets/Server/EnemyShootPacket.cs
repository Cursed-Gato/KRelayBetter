using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class EnemyShootPacket : Packet
    {
        public float Angle;
        public float AngleInc;
        public ushort BulletId;
        public int BulletType;
        public short Damage;
        public Location Position;
        public int NumShots;
        public int OwnerId;

        public override PacketType Type => PacketType.ENEMYSHOOT;

        public override void Read(PacketReader r)
        {
            BulletId = r.ReadUInt16();
            OwnerId = r.ReadInt32();
            BulletType = r.ReadByte();
            Position = (Location)new Location().Read(r);
            Angle = r.ReadSingle();
            Damage = r.ReadInt16();

            if (r.BaseStream.Position < r.BaseStream.Length)
            {
                NumShots = r.ReadByte();
                AngleInc = r.ReadSingle();
            }
            else
            {
                NumShots = -1;
                AngleInc = 0.0f;
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(BulletId);
            w.Write(OwnerId);
            w.Write(BulletType);
            Position.Write(w);
            w.Write(Angle);
            w.Write(Damage);

            if (NumShots != -1)
            {
                w.Write(NumShots);
                w.Write(AngleInc);
            }
        }
    }
}
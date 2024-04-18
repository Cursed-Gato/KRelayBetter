using Lib_K_Relay.Networking.Packets.DataObjects.Location;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ServerPlayerShootPacket : Packet
    {
        public float Angle;
        public float AngleInc = -1.0f;
        public ushort BulletId;
        public int ContainerType;
        public short Damage;
        public int OwnerId;
        public byte NumShots = 0;
        public Location StartingLoc;
        public int SuperOwnerId;
        public byte BulletType = 255;

        public override PacketType Type => PacketType.SERVERPLAYERSHOOT;

        public override void Read(PacketReader r)
        {
            BulletId = r.ReadUInt16();
            OwnerId = r.ReadInt32();
            ContainerType = r.ReadInt32();
            StartingLoc = (Location)new Location().Read(r);
            Angle = r.ReadSingle();
            Damage = r.ReadInt16();
            SuperOwnerId = r.ReadInt32();

            if(r.BaseStream.Position != r.BaseStream.Length)
            {
                BulletType = r.ReadByte();

                if (r.BaseStream.Position != r.BaseStream.Length)
                {
                    NumShots = r.ReadByte();

                    if (r.BaseStream.Position != r.BaseStream.Length)
                    {
                        AngleInc = r.ReadSingle();
                    }
                }
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(BulletId);
            w.Write(OwnerId);
            w.Write(ContainerType);
            StartingLoc.Write(w);
            w.Write(Angle);
            w.Write(Damage);
            w.Write(SuperOwnerId);

            if (BulletType != 255)
            {
                w.Write(BulletType);
                
                if(NumShots != 0)
                {
                    w.Write(NumShots);

                    if(AngleInc != -1.0f)
                    {
                        w.Write(AngleInc);
                    }
                }
            }
        }
    }
}
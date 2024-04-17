using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
	public class PlayerShootPacket : Packet
	{
		 public int Time;
        public ushort BulletId;
        public short ContainerType;
        public sbyte AttackIndex;
        public Location ProjectilePosition;
        public float Angle;
        public byte BurstId;
        public sbyte PatternIdx;
        public sbyte AttackType;
        public Location PlayerPosition;

        public override PacketType Type => PacketType.PLAYERSHOOT;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            BulletId = r.ReadUInt16();
            ContainerType = r.ReadInt16();
            AttackIndex = r.ReadSByte();
            ProjectilePosition = (Location)new Location().Read(r);
            Angle = r.ReadSingle();
            BurstId = r.ReadByte();
            PatternIdx = r.ReadSByte();
            AttackType = r.ReadSByte();
            PlayerPosition = (Location)new Location().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            w.Write(BulletId);
            w.Write(ContainerType);
            w.Write(AttackIndex);
            ProjectilePosition.Write(w);
            w.Write(Angle);
            w.Write(BurstId);
            w.Write(PatternIdx);
            w.Write(AttackType);
            PlayerPosition.Write(w);
        }
	}
}
namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ExaltationBonusChangedPacket : Packet
    {
        public short ObjectType;
        public int AttackProgress;
        public int DefenseProgress;
        public int DexterityProgress;
        public int HealthProgress;
        public int ManaProgress;
        public int SpeedProgress;
        public int VitalityProgress;
        public int WisdomProgress;

        public override PacketType Type => PacketType.EXALTATIONBONUSCHANGED;

        public override void Read(PacketReader r)
        {
            ObjectType = r.ReadInt16();
            DexterityProgress = r.ReadCompressedInt();
            SpeedProgress = r.ReadCompressedInt();
            VitalityProgress = r.ReadCompressedInt();
            WisdomProgress = r.ReadCompressedInt();
            DefenseProgress = r.ReadCompressedInt();
            AttackProgress = r.ReadCompressedInt();
            ManaProgress = r.ReadCompressedInt();
            HealthProgress = r.ReadCompressedInt();
        }

        public override void Write(PacketWriter w)
        {
            w.WriteCompressedInt(ObjectType);
            w.WriteCompressedInt(DexterityProgress);
            w.WriteCompressedInt(SpeedProgress);
            w.WriteCompressedInt(VitalityProgress);
            w.WriteCompressedInt(WisdomProgress);
            w.WriteCompressedInt(DefenseProgress);
            w.WriteCompressedInt(AttackProgress);
            w.WriteCompressedInt(ManaProgress);
            w.WriteCompressedInt(HealthProgress);
        }
    }
}
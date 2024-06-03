namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CreatePacket : Packet
    {
        public short ClassType;
        public short SkinType;
        public bool FirstSession;
        public bool SeasonalChar;

        public override PacketType Type => PacketType.CREATE;

        public override void Read(PacketReader r)
        {
            ClassType = r.ReadInt16();
            SkinType = r.ReadInt16();
            FirstSession = r.ReadBoolean();
            SeasonalChar = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ClassType);
            w.Write(SkinType);
            w.Write(FirstSession);
            w.Write(SeasonalChar);
        }
    }
}
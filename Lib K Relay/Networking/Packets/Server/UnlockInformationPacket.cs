namespace Lib_K_Relay.Networking.Packets.Server
{
    public class UnlockInformationPacket : Packet
    {
        public int UnlockType;

        public override PacketType Type => PacketType.UNLOCKNEWSLOT;

        public override void Read(PacketReader r)
        {
            UnlockType = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(UnlockType);
        }
    }
}
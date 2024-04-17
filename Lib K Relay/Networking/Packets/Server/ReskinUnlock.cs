namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ReskinUnlock : Packet
    {
        public int IsPetSkin; // ??? why is it int
        public int SkinId;

        public override PacketType Type => PacketType.RESKIN;

        public override void Read(PacketReader r)
        {
            SkinId = r.ReadInt32();
            IsPetSkin = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(SkinId);
            w.Write(IsPetSkin);
        }
    }
}
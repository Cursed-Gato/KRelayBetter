namespace Lib_K_Relay.Networking.Packets.Server
{
    public class KeyInfoResponsePacket : Packet
    {
        public string Creator;
        public string Description;
        public string Name;

        public override PacketType Type => PacketType.KEYINFORESPONSE;

        public override void Read(PacketReader r)
        {
            Name = r.ReadString();
            Description = r.ReadString();
            Creator = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Name);
            w.Write(Description);
            w.Write(Creator);
        }
    }
}
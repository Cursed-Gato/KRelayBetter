namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ChangeAllyShootPacket : Packet
    {
        public int Toggle;

        public override PacketType Type => PacketType.CHANGEALLYSHOOT;

        public override void Read(PacketReader r)
        {
            Toggle = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Toggle);
        }
    }
}
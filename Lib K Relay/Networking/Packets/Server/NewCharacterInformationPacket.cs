namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NewCharacterInformationPacket : Packet
    {
        public string CharacterXml;

        public override PacketType Type => PacketType.NEWCHARACTERINFORMATION;

        public override void Read(PacketReader r)
        {
            CharacterXml = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CharacterXml);
        }
    }
}
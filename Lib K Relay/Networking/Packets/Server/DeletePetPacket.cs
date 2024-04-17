namespace Lib_K_Relay.Networking.Packets.Server
{
    public class DeletePetPacket : Packet
    {
        public int PetId;

        public override PacketType Type => PacketType.DELETEPET;

        public override void Read(PacketReader r)
        {
            PetId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PetId);
        }
    }
}
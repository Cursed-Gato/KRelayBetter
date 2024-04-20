
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class FavourPetPacket : Packet
    {
        public int PetId;

        public override PacketType Type => PacketType.FAVORPET;

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

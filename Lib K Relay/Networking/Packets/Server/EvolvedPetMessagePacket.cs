namespace Lib_K_Relay.Networking.Packets.Server
{
    public class EvolvedPetMessagePacket : Packet
    {
        public int FinalSkin;
        public int InitialSkin;
        public int PetId;

        public override PacketType Type => PacketType.EVOLVEDPET;

        public override void Read(PacketReader r)
        {
            PetId = r.ReadInt32();
            InitialSkin = r.ReadInt32();
            FinalSkin = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PetId);
            w.Write(InitialSkin);
            w.Write(FinalSkin);
        }
    }
}
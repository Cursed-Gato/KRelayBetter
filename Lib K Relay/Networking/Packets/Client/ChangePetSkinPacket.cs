namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ChangePetSkinPacket : Packet
    {
        public int Currency;
        public int PetId;
        public int SkinType;

        public override PacketType Type => PacketType.CHANGEPETSKIN;

        public override void Read(PacketReader r)
        {
            PetId = r.ReadInt32();
            SkinType = r.ReadInt32();
            Currency = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PetId);
            w.Write(SkinType);
            w.Write(Currency);
        }
    }
}
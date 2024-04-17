namespace Lib_K_Relay.Networking.Packets.Server
{
    public class HatchEggPacket : Packet
    {
        public int ItemType;
        public string PetName;
        public int PetSkinId;

        public override PacketType Type => PacketType.HATCHPET;

        public override void Read(PacketReader r)
        {
            PetName = r.ReadString();
            PetSkinId = r.ReadInt32();
            ItemType = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PetName);
            w.Write(PetSkinId);
            w.Write(ItemType);
        }
    }
}
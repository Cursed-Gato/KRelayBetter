namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ActivePetUpdateRequestPacket : Packet
    {
        public const byte FollowPet = 1;
        public const byte UnfollowPet = 2;
        public const byte ReleasePet = 3;

        public byte CommandId;
        public int PetId;

        public override PacketType Type => PacketType.ACTIVEPETUPDATEREQUEST;

        public override void Read(PacketReader r)
        {
            CommandId = r.ReadByte();
            PetId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CommandId);
            w.Write(PetId);
        }
    }
}
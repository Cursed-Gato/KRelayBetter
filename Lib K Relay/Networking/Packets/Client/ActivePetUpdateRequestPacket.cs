namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ActivePetUpdateRequestPacket : Packet
    {
        public const int FollowPet = 1;
        public const int UnfollowPet = 2;
        public const int ReleasePet = 3;

        public int CommandId;
        public uint PetId;

        public override PacketType Type => PacketType.ACTIVEPETUPDATEREQUEST;

        public override void Read(PacketReader r)
        {
            CommandId = r.ReadByte();
            PetId = (uint)r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((byte)CommandId);
            w.Write((int)PetId);
        }
    }
}
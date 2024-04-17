using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NewAbilityUnlockedPacket : Packet
    {
        public Ability AbilityType;

        public override PacketType Type => PacketType.NEWABILITY;

        public override void Read(PacketReader r)
        {
            AbilityType = (Ability)r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((int)Type);
        }
    }
}
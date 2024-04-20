
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UpgradeEnchantmentPacket : Packet
    {
        public short UNKNOWN1;
        public byte SlotIdx;

        public override PacketType Type => PacketType.UPGRADEENCHANTMENT;

        public override void Read(PacketReader r)
        {
            UNKNOWN1 = r.ReadInt16();
            SlotIdx = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(UNKNOWN1);
            w.Write(SlotIdx);
        }
    }
}

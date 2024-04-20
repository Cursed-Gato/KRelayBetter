
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UpgradeEnchanterPacket : Packet
    {
        public byte DustType;
        public byte CurrencyType;

        public override PacketType Type => PacketType.UPGRADEENCHANTER;

        public override void Read(PacketReader r)
        {
            DustType = r.ReadByte();
            CurrencyType = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(DustType);
            w.Write(CurrencyType);
        }
    }
}

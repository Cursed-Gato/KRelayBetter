
namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ForgeUnlockedBlueprintsPacket : Packet
    {
        public byte SeasonalForge;
        public int[] UnlockedItems;

        public override PacketType Type => PacketType.FORGEUNLOCKEDBLUEPRINTS;

        public override void Read(PacketReader r)
        {
            SeasonalForge = r.ReadByte();
            UnlockedItems = new int[r.ReadCompressedInt()];
            for (var i = 0; i < UnlockedItems.Length; i++)
                UnlockedItems[i] = r.ReadCompressedInt();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(SeasonalForge);
            w.WriteCompressedInt((byte)UnlockedItems.Length);
            foreach (var item in UnlockedItems)
                w.WriteCompressedInt(item);
        }
    }
}
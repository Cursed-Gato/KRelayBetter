
using Lib_K_Relay.Networking.Packets.DataObjects;
using MetroFramework.Drawing.Html;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class BuyCustomisationSocketPacket : Packet
    {
        public ItemBuyData[] Items;

        public override PacketType Type => PacketType.BUYITEM;

        public override void Read(PacketReader r)
        {
            Items = new ItemBuyData[r.ReadInt16()];
            for(int i  = 0; i < Items.Length; i++)
            {
                Items[i] = (ItemBuyData)new ItemBuyData().Read(r);
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)Items.Length);
            for(int i = 0;i < Items.Length;i++)
            {
                Items[i].Write(w);
            }
        }
    }
}

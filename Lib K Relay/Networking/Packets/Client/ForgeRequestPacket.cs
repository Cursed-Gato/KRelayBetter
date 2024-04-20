using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ForgeRequestPacket : Packet
    {
        public int ResultItemType;
        public SlotObject[] DismantledItems;

        public override PacketType Type => PacketType.FORGEREQUEST;

        public override void Read(PacketReader r)
        {
            ResultItemType = r.ReadInt32();
            DismantledItems = new SlotObject[r.ReadInt32()];
            foreach (var offer in DismantledItems)
                offer.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ResultItemType);
            w.Write(DismantledItems.Length);
            foreach (var offer in DismantledItems)
                offer.Write(w);
        }
    }
}
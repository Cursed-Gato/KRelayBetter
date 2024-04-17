using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ForgeRequestPacket : Packet
    {
        public int CraftItemId;
        public SlotObject[] Offers;

        public override PacketType Type => PacketType.FORGEREQUEST;

        public override void Read(PacketReader r)
        {
            CraftItemId = r.ReadInt32();
            Offers = new SlotObject[r.ReadInt32()];
            foreach (var offer in Offers)
                offer.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CraftItemId);
            w.Write(Offers.Length);
            foreach (var offer in Offers)
                offer.Write(w);
        }
    }
}
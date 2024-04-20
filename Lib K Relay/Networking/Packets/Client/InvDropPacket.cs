using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class InvDropPacket : Packet
    {
        public SlotObject Slot;
        public bool QuickSlot;

        public override PacketType Type => PacketType.INVENTORYDROP;

        public override void Read(PacketReader r)
        {
            Slot = (SlotObject)new SlotObject().Read(r);
            QuickSlot = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            Slot.Write(w);
            w.Write(QuickSlot);
        }
    }
}
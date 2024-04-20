using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class QuestRedeemPacket : Packet
    {
        public int ItemId;
        public string QuestId;
        public SlotObject[] Slots;

        public override PacketType Type => PacketType.QUESTREDEEM;

        public override void Read(PacketReader r)
        {
            QuestId = r.ReadString();
            ItemId = r.ReadInt32();
            Slots = new SlotObject[r.ReadInt16()];
            for(int i = 0; i < Slots.Length; i++)
                Slots[i].Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(QuestId);
            w.Write(ItemId);
            w.Write((short)Slots.Length);
            for (int i = 0; i < Slots.Length; i++)
                Slots[i].Write(w);
        }
    }
}
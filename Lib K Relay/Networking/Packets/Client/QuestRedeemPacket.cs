using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

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
            foreach (var obj in Slots)
                obj.Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(QuestId);
            w.Write(ItemId);
            w.Write((short)Slots.Length);
            foreach (var obj in Slots)
                obj.Write(w);
        }
    }
}
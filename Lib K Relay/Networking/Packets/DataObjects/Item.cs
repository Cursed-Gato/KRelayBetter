namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class Item : IDataObject
    {
        public bool Included;
        public int ItemId;
        public int SlotType;
        public bool Tradable;
        public string UniqueData;

        public IDataObject Read(PacketReader r)
        {
            ItemId = r.ReadInt32();
            SlotType = r.ReadInt32();
            Tradable = r.ReadBoolean();
            Included = r.ReadBoolean();
            UniqueData = r.ReadString();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(ItemId);
            w.Write(SlotType);
            w.Write(Tradable);
            w.Write(Included);
            w.Write(UniqueData);
        }

        public object Clone()
        {
            return new Item
            {
                ItemId = ItemId,
                SlotType = SlotType,
                Tradable = Tradable,
                Included = Included,
                UniqueData = UniqueData
            };
        }

        public override string ToString()
        {
            return "{ ItemId=" + ItemId + ", SlotType=" + SlotType + ", Tradable=" + Tradable + ", Included=" +
                   Included + ", UniqueData=" + UniqueData + "}";
        }
    }
}
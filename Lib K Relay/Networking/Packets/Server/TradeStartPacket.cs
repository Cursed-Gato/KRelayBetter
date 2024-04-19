using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class TradeStartPacket : Packet
    {
        public Item[] MyItems;
        public string YourName;
        public Item[] YourItems;
        public int YourObjectId;
        public byte something;

        public override PacketType Type => PacketType.TRADESTART;

        public override void Read(PacketReader r)
        {
            MyItems = new Item[r.ReadInt16()];
            for (var i = 0; i < MyItems.Length; i++) MyItems[i] = (Item)new Item().Read(r);

            YourName = r.ReadString();
            YourItems = new Item[r.ReadInt16()];
            for (var i = 0; i < YourItems.Length; i++) YourItems[i] = (Item)new Item().Read(r);

            YourObjectId = r.ReadInt32();
            something = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)MyItems.Length);
            foreach (var i in MyItems) i.Write(w);

            w.Write(YourName);
            w.Write((short)YourItems.Length);
            foreach (var i in YourItems) i.Write(w);

            w.Write(YourObjectId);
            w.Write(something);
        }
    }
}
using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class InvSwapPacket : Packet
    {
        public Location Position;
        public SlotObject SlotObject1;
        public SlotObject SlotObject2;
        public int Time;

        public override PacketType Type => PacketType.INVENTORYSWAP;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            Position = (Location)new Location().Read(r);
            SlotObject1 = (SlotObject)new SlotObject().Read(r);
            SlotObject2 = (SlotObject)new SlotObject().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            Position.Write(w);
            SlotObject1.Write(w);
            SlotObject2.Write(w);
        }
    }
}
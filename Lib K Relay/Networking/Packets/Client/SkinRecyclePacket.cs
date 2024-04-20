using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class SkinRecyclePacket : Packet
    {
        public SlotObject SlotObjectData;

        public override PacketType Type => PacketType.SKINRECYCLE;

        public override void Read(PacketReader r)
        {
            SlotObjectData = (SlotObject)new SlotObject().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            SlotObjectData.Write(w);
        }
    }
}

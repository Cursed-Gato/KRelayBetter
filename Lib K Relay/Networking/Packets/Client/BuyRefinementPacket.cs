using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class BuyRefinementPacket : Packet
    {
        public SlotObject SlotObjectData;
        public RefineAction Action;

        public enum RefineAction
        {
            Upgrade = 1,
            Downgrade = 2,
            Reroll = 3,
            Wipe = 4,
        }

    public override PacketType Type => PacketType.BUYREFINEMENT;

        public override void Read(PacketReader r)
        {
            SlotObjectData = (SlotObject)new SlotObject().Read(r);
            Action = (RefineAction)r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            SlotObjectData.Write(w);
            w.Write((short)Action);
        }
    }
}

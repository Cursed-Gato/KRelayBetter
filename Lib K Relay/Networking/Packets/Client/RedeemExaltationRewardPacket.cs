namespace Lib_K_Relay.Networking.Packets.Client
{
    public class RedeemExaltationRewardPacket : Packet
    {
        public int ItemId;

        public override PacketType Type => PacketType.REDEEMEXALTATIONREWARD;

        public override void Read(PacketReader r)
        {
            ItemId = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ItemId);
        }
    }
}
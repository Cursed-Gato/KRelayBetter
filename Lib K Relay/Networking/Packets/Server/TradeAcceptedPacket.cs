namespace Lib_K_Relay.Networking.Packets.Server
{
    public class TradeAcceptedPacket : Packet
    {
        public bool[] MyOffers;
        public bool[] YourOffers;

        public override PacketType Type => PacketType.TRADEACCEPTED;

        public override void Read(PacketReader r)
        {
            MyOffers = new bool[r.ReadInt16()];
            for (var i = 0; i < MyOffers.Length; i++) MyOffers[i] = r.ReadBoolean();

            YourOffers = new bool[r.ReadInt16()];
            for (var i = 0; i < YourOffers.Length; i++) YourOffers[i] = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)MyOffers.Length);
            foreach (var i in MyOffers) w.Write(i);

            w.Write((short)YourOffers.Length);
            foreach (var i in YourOffers) w.Write(i);
        }
    }
}
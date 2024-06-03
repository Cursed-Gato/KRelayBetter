namespace Lib_K_Relay.Networking.Packets.Client
{
    public class AcceptTradePacket : Packet
    {
        public byte[] MyOffers;
        public byte[] YourOffers;

        public override PacketType Type => PacketType.ACCEPTTRADE;

        public override void Read(PacketReader r)
        {
            MyOffers = new byte[r.ReadInt16()];
            for (var i = 0; i < MyOffers.Length; i++) MyOffers[i] = r.ReadByte();

            YourOffers = new byte[r.ReadInt16()];
            for (var i = 0; i < YourOffers.Length; i++) YourOffers[i] = r.ReadByte();
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
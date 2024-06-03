namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ChangeTradePacket : Packet
    {
        public byte[] Offers;

        public override PacketType Type => PacketType.CHANGETRADE;

        public override void Read(PacketReader r)
        {
            Offers = new byte[r.ReadInt16()];
            for (var i = 0; i < Offers.Length; i++) Offers[i] = r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)Offers.Length);
            foreach (var i in Offers) w.Write(i);
        }
    }
}
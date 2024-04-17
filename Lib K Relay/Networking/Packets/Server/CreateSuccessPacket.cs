namespace Lib_K_Relay.Networking.Packets.Server
{
    public class CreateSuccessPacket : Packet
    {
        public int CharId;
        public int ObjectId;
        public string Stats;

        public override PacketType Type => PacketType.CREATESUCCESS;

        public override void Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
            CharId = r.ReadInt32();
            Stats = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectId);
            w.Write(CharId);
            w.Write(Stats);
        }
    }
}
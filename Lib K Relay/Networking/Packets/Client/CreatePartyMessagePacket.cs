namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CreatePartyMessagePacket : Packet
    {
        string Description;
        ushort Powerlevel;
        byte PartySize;
        byte Activity;
        byte MaxedStats;
        byte Serverdropdownlist;
        byte Privacy;

        public override PacketType Type => PacketType.CREATEPARTYMESSAGE;

        public override void Read(PacketReader r)
        {
            Description = r.ReadString();
            Powerlevel = r.ReadUInt16();
            PartySize = r.ReadByte();
            Activity = r.ReadByte();
            MaxedStats = r.ReadByte();
            Serverdropdownlist = r.ReadByte();
            Privacy = r.ReadByte();   

        }

        public override void Write(PacketWriter w)
        {
            w.Write(Description);
            w.Write(Powerlevel);
            w.Write(PartySize);
            w.Write(Activity);
            w.Write(MaxedStats);
            w.Write(Serverdropdownlist);
            w.Write(Privacy);
        }
    }
}

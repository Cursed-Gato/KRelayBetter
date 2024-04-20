
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class EmotePacket : Packet
    {
        public int EmoteType;
        public int Time;
        public bool Automatic;

        public override PacketType Type => PacketType.EMOTE;

        public override void Read(PacketReader r)
        {
            EmoteType = r.ReadInt32();
            Time = r.ReadInt32();
            Automatic = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(EmoteType);
            w.Write(Time);
            w.Write(Automatic);
        }
    }
}

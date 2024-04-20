using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class StartUsePacket : Packet
    {
        public int Time;
        public Location StartPos;
        public Location EndPos;

        public override PacketType Type => PacketType.STARTUSE;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            StartPos = (Location)new Location().Read(r);
            EndPos = (Location)new Location().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            StartPos.Write(w);
            EndPos.Write(w);
        }
    }
}

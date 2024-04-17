using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CreepMoveMessagePacket : Packet
    {
        public bool Hold;
        public Location Position = new Location();
        public int Time;

        public override PacketType Type => PacketType.CREEPMOVE;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            Position.Read(r);
            Hold = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            Position.Write(w);
            w.Write(Hold);
        }
    }
}
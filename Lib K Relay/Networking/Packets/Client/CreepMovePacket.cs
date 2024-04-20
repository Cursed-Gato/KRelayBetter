using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class CreepMovePacket : Packet
    {
        public int CreepId; // 0x20
        public int Time; // 0x24
        Location Position; // 0x2C
        public bool Teleported; // 0x30

        public override PacketType Type => PacketType.CREEPMOVE;

        public override void Read(PacketReader r)
        {
            CreepId = r.ReadInt32();
            Time = r.ReadInt32();
            Position.Read(r);
            Teleported = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CreepId);
            w.Write(Time);
            Position.Write(w);
            w.Write(Teleported);
        }
    }
}
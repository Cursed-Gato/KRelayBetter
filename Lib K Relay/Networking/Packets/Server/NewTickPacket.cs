using Lib_K_Relay.Networking.Packets.DataObjects.Stat;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NewTickPacket : Packet
    {
        public ushort ServerLastRttms;
        public uint ServerRealTimeMs;
        public Status[] Statuses;
        public int TickId;
        public int TickTime;

        public override PacketType Type => PacketType.NEWTICK;

        public override void Read(PacketReader r)
        {
            TickId = r.ReadInt32();
            TickTime = r.ReadInt32();
            ServerRealTimeMs = r.ReadUInt32();
            ServerLastRttms = r.ReadUInt16();

            Statuses = new Status[r.ReadInt16()];
            for (var i = 0; i < Statuses.Length; i++) 
                Statuses[i] = (Status)new Status().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(TickId);
            w.Write(TickTime);
            w.Write(ServerRealTimeMs);
            w.Write(ServerLastRttms);

            w.Write((short)Statuses.Length);
            foreach (var s in Statuses)
                s.Write(w);
        }
    }
}
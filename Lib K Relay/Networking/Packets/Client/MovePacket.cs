using Lib_K_Relay.Networking.Packets.DataObjects.Location;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class MovePacket : Packet
    {
        public LocationRecord[] Records;
        public int TickId;
        public uint ServerRealTimeMSofLastNewTick;

        public override PacketType Type => PacketType.MOVE;

        public override void Read(PacketReader r)
        {
            TickId = r.ReadInt32();
            ServerRealTimeMSofLastNewTick = r.ReadUInt32();

            Records = new LocationRecord[r.ReadInt16()];
            for (var i = 0; i < Records.Length; i++) 
                Records[i] = (LocationRecord)new LocationRecord().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(TickId);
            w.Write(ServerRealTimeMSofLastNewTick);

            w.Write((short)Records.Length);
            foreach (var l in Records)
                l.Write(w);

            PluginUtils.Log("Nig", ToString());
        }
    }
}
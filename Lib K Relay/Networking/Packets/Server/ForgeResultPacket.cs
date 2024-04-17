using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ForgeResultPacket : Packet
    {
        public SlotObject[] Results;
        public bool Success;

        public override PacketType Type => PacketType.FORGERESULT;

        public override void Read(PacketReader r)
        {
            Success = r.ReadBoolean();
            Results = new SlotObject[r.ReadByte()];
            for (var i = 0; i < Results.Length; i++)
                Results[i].Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Success);
            w.Write((byte)Results.Length);
            foreach (var obj in Results)
                obj.Write(w);
        }
    }
}
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class QuestObjIdPacket : Packet
    {
        public int ObjectId;
        public int[] Unknown;

        public override PacketType Type => PacketType.QUESTOBJECTID;

        public override void Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
            Unknown = new int[r.ReadCompressedInt()];
            for (var i = 0; i < Unknown.Length; i++) Unknown[i] = r.ReadCompressedInt();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ObjectId);
            w.WriteCompressedInt(Unknown.Length);
            foreach (var i in Unknown)
                w.WriteCompressedInt(i);
        }
    }
}
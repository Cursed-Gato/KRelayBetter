using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class InvResultPacket : Packet
    {
        public SlotObject FromSlotObject = new SlotObject();
        public bool Result;
        public byte ResultType;
        public SlotObject ToSlotObject = new SlotObject();
        public int[] Conditions;

        public override PacketType Type => PacketType.INVENTORYRESULT;

        public override void Read(PacketReader r)
        {
            Result = r.ReadBoolean();
            ResultType = r.ReadByte();
            FromSlotObject.Read(r);
            ToSlotObject.Read(r);
            
            Conditions = new int[r.ReadInt32()];
            for(int i = 0; i < Conditions.Length; i++)
            {
                Conditions[i] = r.ReadInt32();
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Result);
            w.Write(ResultType);
            FromSlotObject.Write(w);
            ToSlotObject.Write(w);

            w.Write(Conditions.Length);
            for(int i = 0 ; i < Conditions.Length; i++)
            {
                w.Write(Conditions[i]);
            }
        }
    }
}
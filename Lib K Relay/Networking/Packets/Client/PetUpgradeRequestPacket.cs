using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PetUpgradeRequestPacket : Packet
    {
        public byte PetTransactionType;
        public int PIDOne;
        public int PIDTwo;
        public int ObjectId;
        public byte PaymentTransactionType;
        public SlotObject[] SlotObjects;

        public override PacketType Type => PacketType.PETUPGRADEREQUEST;

        public override void Read(PacketReader r)
        {
            PetTransactionType = r.ReadByte();
            PIDOne = r.ReadInt32();
            PIDTwo = r.ReadInt32();
            ObjectId = r.ReadInt32();
            PaymentTransactionType = r.ReadByte();
            
            SlotObjects = new SlotObject[r.ReadInt16()];
            for (int i = 0; i < SlotObjects.Length; i++)
            {
                SlotObjects[i] = (SlotObject)new SlotObject().Read(r);
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PetTransactionType);
            w.Write(PIDOne);
            w.Write(PIDTwo);
            w.Write(ObjectId);
            w.Write(PaymentTransactionType);
            
            w.Write((short)SlotObjects.Length);
            foreach (var slot in SlotObjects)
            {
                slot.Write(w);
            }
        }
    }
}

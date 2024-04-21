namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class SlotObject : IDataObject
    {
        public int ObjectId;
        public int ObjectType;
        public int SlotId;

        public SlotObject()
        {

        }

        public SlotObject(int objectId, int objectType, int slotId)
        {
            ObjectId = objectId;
            ObjectType = objectType;
            SlotId = slotId;
        }

        public IDataObject Read(PacketReader r)
        {
            ObjectId = r.ReadInt32();
            SlotId = r.ReadInt32();
            ObjectType = r.ReadInt32();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(ObjectId);
            w.Write(SlotId);
            w.Write(ObjectType);
        }

        public object Clone()
        {
            return new SlotObject(ObjectId, ObjectType, SlotId);
        }

        public override string ToString()
        {
            return "{ ObjectId=" + ObjectId + ", SlotId=" + SlotId + ", ObjectType=" + ObjectType + " }";
        }
    }
}
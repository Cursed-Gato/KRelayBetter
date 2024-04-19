

namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class PartyPlayer : IDataObject
    {
        public ushort Id;
        public string Name;
        public ushort ObjectId;
        public ushort UNKNOWN1;

        public IDataObject Read(PacketReader r)
        {
            Id = r.ReadUInt16();
            Name = r.ReadString();
            ObjectId = r.ReadUInt16();
            UNKNOWN1 = r.ReadUInt16();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(Id);
            w.Write(Name);
            w.Write(ObjectId);
            w.Write(UNKNOWN1);
        }

        public object Clone()
        {
            return new PartyPlayer
            {
                Id = Id,
                Name = Name,
                ObjectId = ObjectId,
                UNKNOWN1 = UNKNOWN1
            };
        }

        public override string ToString()
        {
            return "{ Id=" + Id + ", Name=" + Name + ", ObjectId=" + ObjectId + ", UNKNOWN1=" +
                   UNKNOWN1 + " }";
        }
    }
}



namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class PartyPlayer : IDataObject
    {
        public ushort UNKNOWN2;
        public string UNKNOWN3;
        public ushort UNKNOWN4;
        public ushort UNKNOWN1;

        public IDataObject Read(PacketReader r)
        {
            UNKNOWN2 = r.ReadUInt16();
            UNKNOWN3 = r.ReadString();
            UNKNOWN4 = r.ReadUInt16();
            UNKNOWN1 = r.ReadUInt16();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(UNKNOWN2);
            w.Write(UNKNOWN3);
            w.Write(UNKNOWN4);
            w.Write(UNKNOWN1);
        }

        public object Clone()
        {
            return new PartyPlayer
            {
                UNKNOWN1 = UNKNOWN1,
                UNKNOWN2 = UNKNOWN2,
                UNKNOWN3 = UNKNOWN3,
                UNKNOWN4 = UNKNOWN4
            };
        }

        public override string ToString()
        {
            return "{ ItemId=" + UNKNOWN1 + ", SlotType=" + UNKNOWN2 + ", Tradable=" + UNKNOWN3 + ", Included=" +
                   UNKNOWN4 + " }";
        }
    }
}


namespace Lib_K_Relay.Networking.Packets.DataObjects
{
	public class Party : IDataObject
	{
		public string UNKNOWN2;
		public uint UNKNOWN3;
		public ushort UNKNOWN4;
		public byte UNKNOWN5;
		public byte UNKNOWN6;
		public byte UNKNOWN7;
		public byte UNKNOWN8;
		public byte UNKNOWN9;
		public byte UNKNOWN1;

		public IDataObject Read(PacketReader r)
		{
			UNKNOWN2 = r.ReadString();
			UNKNOWN3 = r.ReadUInt32();
			UNKNOWN4 = r.ReadUInt16();
			UNKNOWN5 = r.ReadByte();
			UNKNOWN6 = r.ReadByte();
			UNKNOWN7 = r.ReadByte();
			UNKNOWN8 = r.ReadByte();
			UNKNOWN9 = r.ReadByte();
			UNKNOWN1 = r.ReadByte();

			return this;
		}

		public void Write(PacketWriter w)
		{
			w.Write(UNKNOWN2);
			w.Write(UNKNOWN3);
			w.Write(UNKNOWN4);
			w.Write(UNKNOWN5);
			w.Write(UNKNOWN6);
			w.Write(UNKNOWN7);
			w.Write(UNKNOWN8);
			w.Write(UNKNOWN9);
            w.Write(UNKNOWN1);
        }


		public object Clone()
		{
			return new Party
			{
				UNKNOWN1 = UNKNOWN1,
				UNKNOWN2 = UNKNOWN2,
				UNKNOWN3 = UNKNOWN3,
				UNKNOWN4 = UNKNOWN4,
				UNKNOWN5 = UNKNOWN5,
				UNKNOWN6 = UNKNOWN6,
				UNKNOWN7 = UNKNOWN7,
				UNKNOWN8 = UNKNOWN8,
				UNKNOWN9 = UNKNOWN9
			};
		}

		public override string ToString()
		{
			return "{ UNKNOWN1=" + UNKNOWN1 + ", UNKNOWN2=" + UNKNOWN2 + ", UNKNOWN3=" + UNKNOWN3 +
				   ", UNKNOWN4=" + UNKNOWN4 + ", UNKNOWN5=" + UNKNOWN5 + ", UNKNOWN6=" + UNKNOWN6 +
				   ", UNKNOWN7=" + UNKNOWN7 + ", UNKNOWN8=" + UNKNOWN8 + ", UNKNOWN9=" + UNKNOWN9 + " }";
		}
	}
}

using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
	public class ClaimChestRewardPacket : Packet
	{
		public bool Accepted;
		public SlotObject SlotObjectData;
		public byte SelectedIdx;

		public override PacketType Type => PacketType.CLAIMCHESTREWARDSUBMIT;

		public override void Read(PacketReader r)
		{
			Accepted = r.ReadBoolean();
			SlotObjectData = (SlotObject)new SlotObject().Read(r);
			SelectedIdx = r.ReadByte();
		}

		public override void Write(PacketWriter w)
		{
			w.Write(Accepted);
			SlotObjectData.Write(w);
			w.Write(SelectedIdx);
		}
	}
}
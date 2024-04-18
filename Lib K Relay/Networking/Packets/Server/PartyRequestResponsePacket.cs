
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class PartyRequestResponsePacket : Packet
    {
        public string PlayerName;
        public ushort ClassId;
        public ushort SkinId;
        public InviteState State;

        public enum InviteState
        {
            None,
            Pending,
            Cancelled,
            Accepted,
            Declined,
            PartyFull,
            Blacklisted
        }

        public override PacketType Type => PacketType.PARTYREQUESTRESPONSE;

        public override void Read(PacketReader r)
        {
            PlayerName = r.ReadString();
            ClassId = r.ReadUInt16();
            SkinId = r.ReadUInt16();
            State = (InviteState)r.ReadByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(PlayerName);
            w.Write(ClassId);
            w.Write(SkinId);
            w.Write((byte)State);

            PluginUtils.Log("Nig", ToString());
        }
    }
}

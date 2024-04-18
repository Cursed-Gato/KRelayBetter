using System;
using System.Collections.Generic;
using System.Linq;
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PartyMemberAddedPacket : Packet
    {
        ushort invitePlayerId;
        string name;
        ushort classId;
        ushort skinId;

        public override PacketType Type => PacketType.PARTYMEMBERADDED;

        public override void Read(PacketReader r)
        {
            invitePlayerId = r.ReadUInt16();
            name = r.ReadString();
            classId = r.ReadUInt16();
            skinId = r.ReadUInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(invitePlayerId);
            w.Write(name);
            w.Write(classId);
            w.Write(skinId);
        }
    }
}

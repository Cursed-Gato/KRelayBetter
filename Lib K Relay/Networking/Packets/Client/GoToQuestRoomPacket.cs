﻿namespace Lib_K_Relay.Networking.Packets.Client
{
    public class GoToQuestRoomPacket : Packet
    {
        public override PacketType Type => PacketType.GOTOQUESTROOM;

        public override void Read(PacketReader r)
        {
        }

        public override void Write(PacketWriter w)
        {
        }
    }
}
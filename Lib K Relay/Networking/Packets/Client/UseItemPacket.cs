﻿using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class UseItemPacket : Packet
    {
        public Location ItemUsePos;
        public SlotObject SlotObject;
        public int Time;
        public bool Unknown;
        public byte UseType;

        public override PacketType Type => PacketType.USEITEM;

        public override void Read(PacketReader r)
        {
            Time = r.ReadInt32();
            SlotObject = (SlotObject)new SlotObject().Read(r);
            ItemUsePos = (Location)new Location().Read(r);
            UseType = r.ReadByte();
            Unknown = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            SlotObject.Write(w);
            ItemUsePos.Write(w);
            w.Write(UseType);
            w.Write(Unknown);
        }
    }
}
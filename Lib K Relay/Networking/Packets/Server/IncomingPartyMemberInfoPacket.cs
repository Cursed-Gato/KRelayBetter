using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class IncomingPartyMemberInfoPacket : Packet
    {
        uint Partyid;
        ushort Unknown2;
        byte MaxSize;
        public PartyPlayer[] PartyPlayer;
        string Description;
        
        public override PacketType Type => PacketType.INCOMINGPARTYMEMBERINFO;

        public override void Read(PacketReader r)
        {
            Partyid = r.ReadUInt32();
            Unknown2 = r.ReadUInt16();
            MaxSize = r.ReadByte();
            PartyPlayer = new PartyPlayer[r.ReadInt16()];
            for (var i = 0; i < PartyPlayer.Length; i++)
                PartyPlayer[i] = (PartyPlayer)new PartyPlayer().Read(r);
            Description = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Partyid);
            w.Write(Unknown2);
            w.Write(MaxSize);
            w.Write((short)PartyPlayer.Length);
            foreach (var i in PartyPlayer)
                i.Write(w);  
            w.Write(Description);
        }
    }
}

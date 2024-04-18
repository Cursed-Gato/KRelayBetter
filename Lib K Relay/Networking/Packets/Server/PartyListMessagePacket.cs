using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class PartyListMessagePacket : Packet
    {
        byte count;
        public Party[] parties;
        public override PacketType Type => PacketType.PARTYLISTMESSAGE;

        public override void Read(PacketReader r)
        {
            count = r.ReadByte();
            parties = new Party[r.ReadInt16()];
            for (var i = 0; i < parties.Length; i++)
                parties[i] = (Party)new Party().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(count);
            w.Write((short)parties.Length);
            foreach (var i in parties)
                i.Write(w);
        }
    }
}

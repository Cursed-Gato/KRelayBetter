using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class DeathPacket : Packet
    {
        public string AccountId;
        public int CharId;
        public string KilledBy;
        public int GravestoneType;
        public int TotalFame;
        public FameData[] Fames;
        public string Stats;

        public override PacketType Type => PacketType.DEATH;

        public override void Read(PacketReader r)
        {
            AccountId = r.ReadString();
            CharId = r.ReadCompressedInt();
            KilledBy = r.ReadString();
            GravestoneType = r.ReadInt32();
            TotalFame = r.ReadCompressedInt();

            Fames = new FameData[r.ReadCompressedInt()];
            for (var i = 0; i < Fames.Length; i++)
                Fames[i] = (FameData) new FameData().Read(r);
        
            Stats = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(AccountId);
            w.WriteCompressedInt(CharId);
            w.Write(KilledBy);
            w.Write(GravestoneType);
            w.WriteCompressedInt(TotalFame);
            w.Write(KilledBy);

            w.WriteCompressedInt(Fames.Length);
            for (var i = 0; i < Fames.Length; i++)
                Fames[i].Write(w);

            w.Write(Stats);
        }
    }
}
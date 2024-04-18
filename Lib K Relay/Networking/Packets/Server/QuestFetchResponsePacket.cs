using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class QuestFetchResponsePacket : Packet
    {
        public DailyQuest[] Quests;
        public short NextRefreshPrice;

        public override PacketType Type => PacketType.QUESTFETCHRESPONSE;

        public override void Read(PacketReader r)
        {
            Quests = new DailyQuest[r.ReadInt16()];
            for(int i = 0; i < Quests.Length; i++)
            {
                Quests[i] = (DailyQuest)new DailyQuest().Read(r);
            }
            NextRefreshPrice = r.ReadInt16();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Quests.Length);
            for(int i = 0;i < Quests.Length; i++) 
            {
                Quests[i].Write(w);
            }
            w.Write(NextRefreshPrice);
        }
    }
}
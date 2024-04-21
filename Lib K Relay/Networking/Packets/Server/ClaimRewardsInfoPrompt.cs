
namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ClaimRewardsInfoPrompt : Packet
    {
        public byte ChestType;
        public int[] Contents;

        public override PacketType Type => PacketType.CLAIMREWARDSINFOPROMPT;

        public override void Read(PacketReader r)
        {
            ChestType = r.ReadByte();

            Contents = new int[r.ReadInt16()];
            for (int i = 0; i < Contents.Length; i++)
            {
                Contents[i] = r.ReadInt32();
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ChestType);

            w.Write((short)Contents.Length);
            for (int i = 0; i < Contents.Length; i++)
            {
                w.Write(Contents[i]);
            }
        }
    }
}

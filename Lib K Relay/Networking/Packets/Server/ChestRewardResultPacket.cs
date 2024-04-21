
namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ChestRewardResultPacket : Packet
    {
        public int[] Contents;

        public override PacketType Type => PacketType.CHESTREWARDRESULT;

        public override void Read(PacketReader r)
        {
            Contents = new int[r.ReadInt16()];
            for(int i = 0; i < Contents.Length; i++)
            {
                Contents[i] = r.ReadInt32();
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)Contents.Length);
            for(int i = 0;i < Contents.Length; i++)
            {
                w.Write(Contents[i]);
            }
        }
    }
}


namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ClaimDailyRewardPacket : Packet
    {
        public string ClaimKey;
        public string ClaimType;

        public override PacketType Type => PacketType.CLAIMDAILYREWARD;

        public override void Read(PacketReader r)
        {
            ClaimKey = r.ReadString();
            ClaimType = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ClaimKey);
            w.Write(ClaimType);
        }
    }
}

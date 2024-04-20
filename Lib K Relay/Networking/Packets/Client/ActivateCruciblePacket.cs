
namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ActivateCruciblePacket : Packet
    {
        public string CrucibleId;
        public bool Activate;

        public override PacketType Type => PacketType.ENABLECRUCIBLE;

        public override void Read(PacketReader r)
        {
            CrucibleId = r.ReadString();
            Activate = r.ReadBoolean();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CrucibleId);
            w.Write(Activate);
        }
    }
}

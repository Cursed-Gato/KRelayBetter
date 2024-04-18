
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ForReconnectPacket : Packet
    {
        public string ReconnectInfo;
        public override PacketType Type => PacketType.FORRECONNECTMESSAGE;

        public override void Read(PacketReader r)
        {
            ReconnectInfo = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(ReconnectInfo);
            PluginUtils.Log("Nig", ToString());
        }
    }
}

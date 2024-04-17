using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.DataObjects.Stat;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace Glow
{
    public class Glow : IPlugin
    {
        public string GetAuthor()
        {
            return "KrazyShank / Kronks";
        }

        public string GetName()
        {
            return "Glow";
        }

        public string GetDescription()
        {
            return "You're so excited about K Relay that you're literally glowing!";
        }

        public string[] GetCommands()
        {
            return new[] { "/glow" };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.HookPacket(PacketType.UPDATE, OnUpdate);
            proxy.HookCommand("glow", (client, cmd, args) =>
            {
                Config.Default.enabled = !Config.Default.enabled;
                Config.Default.Save();
                client.SendToClient(PluginUtils.CreateOryxNotification("Glow",
                    "Glow is now " + (Config.Default.enabled ? "enabled" : "disabled")));
            });
        }

        private void OnUpdate(Client client, Packet packet)
        {
            if (!Config.Default.enabled) return;
            var update = (UpdatePacket)packet;
            for (var i = 0; i < update.NewObjs.Length; i++)
                if (update.NewObjs[i].Status.ObjectId == client.ObjectId)
                    foreach (var t in update.NewObjs[i].Status.Data)
                        if (t.Id == (int)StatsType.Stats.SupporterStat)
                            t.IntValue = 1;
        }
    }
}
using Lib_K_Relay;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace DailyQuestMode
{
    public class DailyQuestMode : IPlugin
    {
        public string GetAuthor()
        {
            return "Animus";
        }

        public string GetName()
        {
            return "DailyQuestMode";
        }

        public string GetDescription()
        {
            return "Connect to daily quest room";
        }

        public string[] GetCommands()
        {
            return new[]
            {
                "dailyquestmode"
            };
        }

        private bool _firstLoad = true;
        private string _currentMap;

        public void Initialize(Proxy proxy)
        {
            proxy.HookCommand("dailyquestmode", (client, cmd, args) =>
            {
                Config.Default.enabled = !Config.Default.enabled;
                Config.Default.Save();
                client.SendToClient(PluginUtils.CreateOryxNotification("Daily Quest Mode",
                    "Daily Quest Mode is now " + (Config.Default.enabled ? "enabled" : "disabled")));
            });

            proxy.HookCommand("dailyquest", (client, cmd, args) =>
            {
                if (_currentMap == "Nexus")
                {
                    client.SendToServer((GoToQuestRoomPacket)Packet.Create(PacketType.GOTOQUESTROOM));
                    return;
                }

                client.SendToServer((EscapePacket)Packet.Create(PacketType.ESCAPE));
            });


            if (Config.Default.enabled)
            {
                proxy.HookPacket(PacketType.MAPINFO,
                    (client, packet) => { _currentMap = ((MapInfoPacket)packet).Name; });

                proxy.HookPacket(PacketType.LOAD, (client, packet) =>
                {
                    if (!_firstLoad && _currentMap != "Nexus") return;
                    packet.Send = false;
                    _firstLoad = false;
                    client.SendToServer((GoToQuestRoomPacket)Packet.Create(PacketType.GOTOQUESTROOM));
                });
            }
        }
    }
}
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Lib_K_Relay;
using Lib_K_Relay.GameData;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace QoLTools
{
    public class QoLTools : IPlugin
    {
        private List<byte> SkipPacketLog;
        private List<byte> PacketsToTrack;
        private Dictionary<int, Packet> TrackedPackets;
        private int PacketIt = 0;
        private bool LogPackets = false;
        private bool LogToClient = false;
        private bool ChestMode = false;


        public string GetAuthor()
        {
            return "Smol";
        }

        public string GetName()
        {
            return "Testing";
        }

        public string GetDescription()
        {
            return "Used to Test/Debug Packets";
        }

        public string[] GetCommands()
        {
            return new[]
            {
                "/logpackets limited:remove:add:stop:start:reset:toclient",
                "/track remove:add:reset:send",
                "/chestmode",
                "/useall {itemid}",
                "/inv"
            };
        }

        public void Initialize(Proxy proxy)
        {
            SkipPacketLog = new List<byte>();
            TrackedPackets = new Dictionary<int, Packet>();
            PacketsToTrack = new List<byte>();

            proxy.ServerPacketRecieved += OnServerPacket;
            proxy.ClientPacketRecieved += OnClientPacket;
            proxy.HookCommand("logpackets", OnLoggerCommamd);
            proxy.HookCommand("track", onTrackCommand);
            proxy.HookCommand("useall", onUseAllCommand);
            proxy.HookCommand("inv", onInvCommand);
            proxy.HookCommand("chestmode", (Client client, string cmd, string[] args) => { ChestMode = !ChestMode; });
            proxy.HookPacket<UseItemPacket>(onUseItem);
            proxy.HookPacket<ClaimRewardsInfoPrompt>(onCliamRewardsInfoItem);
            proxy.HookPacket<ClaimChestRewardPacket>(onCliamRewards);
            proxy.HookPacket<ChestRewardResultPacket>(onChestResultRewards);
        }

        UseItemPacket LastUseItem = null;
        ClaimChestRewardPacket LastChestClaim = null;

        void onUseItem(Client client, UseItemPacket packet)
        {
            LastUseItem = packet;
        }

        void onCliamRewardsInfoItem(Client client, ClaimRewardsInfoPrompt packet)
        {
            if (!ChestMode || LastUseItem == null || LastChestClaim == null)
                return;

            ClaimChestRewardPacket claimPacket = Packet.Create<ClaimChestRewardPacket>(PacketType.CLAIMCHESTREWARDSUBMIT);
            claimPacket.SlotObjectData = LastUseItem.SlotObject;
            claimPacket.SelectedIdx = LastChestClaim.SelectedIdx;
            claimPacket.Accepted = LastChestClaim.Accepted;
            
            client.SendToServer(claimPacket);
            packet.Send = false;
        }
        void onCliamRewards(Client client, ClaimChestRewardPacket packet)
        {
            LastChestClaim = packet;
        }

        void onChestResultRewards(Client client, ChestRewardResultPacket packet)
        {
        }

        public void onInvCommand(Client client, string cmd, string[] args)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < client.PlayerData.Inventory.Length; i++)
            {
                sb.Append(client.PlayerData.Inventory[i].ToString() + ", ");
            }

            client.SendToClient(PluginUtils.CreateOryxNotification("QOL", "Inventory => " + sb.ToString()));

        }

        public void onUseAllCommand(Client client, string cmd, string[] args)
        {
            int itemIdToUse = 0;
            if (!int.TryParse(args[0], out itemIdToUse))
            {
                client.SendToClient(PluginUtils.CreateOryxNotification("QOL", "Failed to Parse ItemId!"));
                return;
            }

            // Create and start a new thread
            Thread thread = new Thread(() =>
            {
                UseItems(client, itemIdToUse);
            });
            thread.Start();
        }

        private void UseItems(Client client, int itemIdToUse)
        {
            for (int i = 0; i < client.PlayerData.Inventory.Length; i++)
            {
                var itemId = client.PlayerData.Inventory[i];
                if (itemId == itemIdToUse)
                {
                    var packetToSend = Packet.Create<UseItemPacket>(PacketType.USEITEM);
                    packetToSend.ItemUsePos = client.PlayerData.Pos;
                    packetToSend.SlotObject = new SlotObject(client.ObjectId, itemId, i);
                    packetToSend.Time = client.Time;

                    client.SendToServer(packetToSend);
                    client.SendToClient(PluginUtils.CreateOryxNotification("QOL", "Used item in slot " + i.ToString() + "!"));
                    LastUseItem = packetToSend;
                    Thread.Sleep(550); // Sleep to simulate delay
                }
            }

            for (int i = 0; i < client.PlayerData.Backpack.Length; i++)
            {
                var itemId = client.PlayerData.Backpack[i];
                if (itemId == itemIdToUse)
                {
                    var packetToSend = Packet.Create<UseItemPacket>(PacketType.USEITEM);
                    packetToSend.ItemUsePos = client.PlayerData.Pos;
                    packetToSend.SlotObject = new SlotObject(client.ObjectId, itemId, i + 12);
                    packetToSend.Time = client.Time;

                    client.SendToServer(packetToSend);
                    client.SendToClient(PluginUtils.CreateOryxNotification("QOL", "Used item in slot " + (i + 12).ToString() + "!"));
                    LastUseItem = packetToSend;
                    Thread.Sleep(550);
                }
            }
        }

        public void onTrackCommand(Client client, string cmd, string[] args)
        {
            if (args[0].ToLower() == "add" || args[0].ToLower() == "remove")
            {
                if (args.Length < 2)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Incorrect Usage, add/remove needs to either have the packetName or the packetId!"));
                    return;
                }

                var packetName = args[1];
                byte packetId = 255;

                if (!byte.TryParse(packetName, out packetId))
                {
                    packetId = GameData.Packets.ByName(packetName.ToUpper()).Id;
                }

                if (!GameData.Packets.Map.ContainsKey(packetId) || packetId == 255)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Failed to get Packet, packetId is incorrect!"));
                }
                else
                {
                    if (args[0].ToLower() == "add")
                    {
                        if (!PacketsToTrack.Contains(packetId))
                            PacketsToTrack.Add(packetId);
                        else
                            client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Already Added!"));
                    }
                    else
                    {
                        if (PacketsToTrack.Contains(packetId))
                            PacketsToTrack.Remove(packetId);
                        else
                            client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Already Removed!"));
                    }
                }
            }

            if (args[0].ToLower() == "reset")
            {
                PacketsToTrack.Clear();
                PacketIt = 0;
                TrackedPackets.Clear();
            }
            if(args[0].ToLower() == "send")
            {
                if (args.Length < 2)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Incorrect Usage, neess to contain packet iterator!"));
                    return;
                }

                byte packetId = 255;
                if (!byte.TryParse(args[1], out packetId))
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Failed to parse packet iterator!"));
                    return;
                }

                if (!TrackedPackets.ContainsKey(packetId))
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Failed to find packet iterator!"));
                    return;
                }

                Packet packetToSend = TrackedPackets[packetId];

                var fields = packetToSend.GetType().GetFields();

                foreach(var fieldInfo in fields)
                {
                    if(fieldInfo.Name.ToLower() == "time")
                    {
                        fieldInfo.SetValue(packetToSend, client.Time);
                    }
                }

                if (args.Length > 2)
                {
                    int timesToSend = 0;
                    int.TryParse(args[2], out timesToSend);
                    for(int i = 0; i < timesToSend; i ++)
                        client.SendToServer(packetToSend);

                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Sent Packet " + timesToSend.ToString() + " Times! -> " + packetToSend.ToString()));
                    return;
                }
                else
                {
                    client.SendToServer(packetToSend);
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Sent Packet! -> " + packetToSend.ToString()));
                }
            }
        }

        public void OnLoggerCommamd(Client client, string cmd, string[] args)
        {
            if (args.Length == 0)
            {
                client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Incorrect Usage, use limited:remove:add:stop:start:reset:toclient!"));
                return;
            }

            if (args[0].ToLower() == "add" || args[0].ToLower() == "remove")
            {
                if (args.Length < 2)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Incorrect Usage, add needs to either have the packetName or the packetId!"));
                    return;
                }

                var packetName = args[1];
                byte packetId = 255;

                if (!byte.TryParse(packetName, out packetId))
                {
                    packetId = GameData.Packets.ByName(packetName.ToUpper()).Id;
                }

                if (!GameData.Packets.Map.ContainsKey(packetId) || packetId == 255)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Failed to get Packet, packetId is incorrect!"));
                }
                else
                {
                    if (args[0].ToLower() == "add")
                    {
                        if (!SkipPacketLog.Contains(packetId))
                            SkipPacketLog.Add(packetId);
                        else
                            client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Already Added!"));
                    }
                    else
                    {
                        if (SkipPacketLog.Contains(packetId))
                            SkipPacketLog.Remove(packetId);
                        else
                            client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Already Removed!"));
                    }
                }
            }

            if (args[0].ToLower() == "start")
                LogPackets = true;
            if (args[0].ToLower() == "stop")
                LogPackets = false;
            if (args[0].ToLower() == "reset")
                SkipPacketLog.Clear();
            if (args[0].ToLower() == "toclient")
                LogToClient = !LogToClient;
            if (args[0].ToLower() == "limited")
            {
                string[] packets = { "hello", "move", "load", "mapinfo", "update", "updateack", "newtick", "ping", "pong", "notification", "text", "showeffect" };
                foreach (var packet in packets)
                {
                    SkipPacketLog.Add(GameData.Packets.ByName(packet.ToUpper()).Id);
                }
            }

        }
        public void LogPacket(Client client, Packet packet)
        {
            if (SkipPacketLog.Contains(packet.Id) || !LogPackets)
                return;

            if (LogToClient)
                client.SendToClient(PluginUtils.CreateOryxNotification("PacketLog", packet.ToString()));
           else
                PluginUtils.Log("PacketLog", packet.ToString());
        }

        public void TrackPacket(Client client, Packet packet)
        {
            if (!PacketsToTrack.Contains(packet.Id))
                return;

            TrackedPackets.Add(PacketIt, packet);
            client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", PacketIt.ToString() + " -> " + packet.Type.ToString()));
            PacketIt++;

        }

        public void OnServerPacket(Client client, Packet packet)
        {
            LogPacket(client, packet);
        }

        public void OnClientPacket(Client client, Packet packet)
        {
            LogPacket(client,packet);
            TrackPacket(client, packet);
        }
    }
}
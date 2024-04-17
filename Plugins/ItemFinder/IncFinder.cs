using System;
using System.Collections.Generic;
using System.Linq;
using Lib_K_Relay;
using Lib_K_Relay.GameData;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.DataObjects.Stat;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace ItemFinder
{
    public class ItemFinder : IPlugin
    {
        private readonly Dictionary<ushort, string> _items = new Dictionary<ushort, string>();
        private bool _findItem;
        private readonly Dictionary<int, string> _itemHolders = new Dictionary<int, string>();

        public string GetAuthor()
        {
            return "KrazyShank / Kronks, Modded by Animus";
        }

        public string GetName()
        {
            return "Item Finder";
        }

        public string GetDescription()
        {
            return "Notifies you when people have an item in their inventories.";
        }

        public string[] GetCommands()
        {
            return new[] { "/setitem", "/finditem", "/clearitems" };
        }

        public void Initialize(Proxy proxy)
        {
            proxy.ClientConnected += client => _itemHolders.Clear();
            proxy.HookPacket(PacketType.UPDATE, OnUpdate);
            proxy.HookCommand("finditem", OnFICommand);
            proxy.HookCommand("setitem", OnSICommand);
            proxy.HookCommand("clearitems", OnCLCommand);
            proxy.HookCommand("test", Test);
        }

        private void OnCLCommand(Client client, string command, string[] args)
        {
            _items.Clear();
            client.SendToClient(PluginUtils.CreateOryxNotification("Item Finder", "Cleared set items list"));
        }

        private void Test(Client client, string command, string[] args)
        {
            foreach (var item in _items) Console.WriteLine(item.Key + " " + item.Value);
        }


        private void OnSICommand(Client client, string command, string[] args)
        {
            if (args.Length == 0) return;

            ushort itemId;
            var item = string.Join(" ", args);
            // Horrible way to do this but it works
            try
            {
                itemId = GameData.Items.Map.First(e => e.Value.Name.ToLower() == item).Value.Id;
                _items.Add(GameData.Items.ById(itemId).Id, GameData.Items.ById(itemId).Name);
            }
            catch (Exception)
            {
                client.SendToClient(PluginUtils.CreateOryxNotification("Item Finder", "Invalid item name."));
                Console.WriteLine(item);
                return;
            }

            client.SendToClient(
                PluginUtils.CreateOryxNotification("Item Finder", "Item set to " + _items[itemId] + "."));
            _findItem = true;
        }

        private void OnFICommand(Client client, string command, string[] args)
        {
            var message = _itemHolders.Aggregate("Item Holders: ", (current, pair) => current + pair.Value + ",");
            client.SendToClient(PluginUtils.CreateOryxNotification("Item Finder", message));
        }

        private void OnUpdate(Client client, Packet packet)
        {
            if (!_findItem) return;

            var update = (UpdatePacket)packet;
            // New Objects
            foreach (var entity in update.NewObjs)
            {
                var hasItem = false;

                foreach (var statData in entity.Status.Data)
                foreach (var item in _items)
                {
                    if (!statData.IsStringData() && statData.Id >= 8 && statData.Id <= 19 ||
                        statData.Id >= 71 && statData.Id <= 78)
                        if (statData.IntValue == item.Key)
                            hasItem = true;

                    if (!hasItem || statData.Id != (int)StatsType.Stats.NameStat ||
                        statData.StringValue == client.PlayerData.Name) continue;

                    if (!_itemHolders.ContainsKey(entity.Status.ObjectId))
                        _itemHolders.Add(entity.Status.ObjectId, statData.StringValue);

                    client.SendToClient(PluginUtils.CreateOryxNotification(
                        "Item Finder", statData.StringValue + " has a " + item.Value + "!"));
                    hasItem = false;
                }
            }

            // Removed Objects
            foreach (var drop in update.Drops)
            {
                if (!_itemHolders.ContainsKey(drop)) continue;
                client.SendToClient(PluginUtils.CreateOryxNotification(
                    "Item Finder", _itemHolders[drop] + " has left!"));
                _itemHolders.Remove(drop);
            }
        }
    }
}
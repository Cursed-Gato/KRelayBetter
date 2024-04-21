using System;
using System.Linq;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking
{
    public class StateManager
    {
        private Proxy _proxy;

        public void Attach(Proxy proxy)
        {
            _proxy = proxy;
            proxy.HookPacket<CreateSuccessPacket>(OnCreateSuccess);
            proxy.HookPacket<MapInfoPacket>(OnMapInfo);
            proxy.HookPacket<UpdatePacket>(OnUpdate);
            proxy.HookPacket<NewTickPacket>(OnNewTick);
            proxy.HookPacket<PlayerShootPacket>(OnPlayerShoot);
            proxy.HookPacket<MovePacket>(OnMove);
        }

        private void OnMove(Client client, MovePacket packet)
        {
            client.PreviousTime = (int)packet.ServerRealTimeMSofLastNewTick;
            client.LastUpdate = Environment.TickCount;
        }

        private void OnPlayerShoot(Client client, PlayerShootPacket packet)
        {
            if(client.relativeTime == 0)
            {
                client.relativeTime = packet.Time - Environment.TickCount;
                client.SendToClient(PluginUtils.CreateOryxNotification("Client", "Synced Time"));
            }
            client.PlayerData.Pos = new Location
            {
                X = packet.ProjectilePosition.X - 0.3f * (float)Math.Cos(packet.Angle),
                Y = packet.ProjectilePosition.Y - 0.3f * (float)Math.Sin(packet.Angle)
            };
        }

        private void OnNewTick(Client client, NewTickPacket packet)
        {
            client.PlayerData.Parse(packet);
        }

        private void OnMapInfo(Client client, MapInfoPacket packet)
        {
            client.State["MapInfo"] = packet;
        }

        private void OnCreateSuccess(Client client, CreateSuccessPacket packet)
        {
            client.PlayerData = new PlayerData(packet.ObjectId, client.State.Value<MapInfoPacket>("MapInfo"));
            PluginUtils.Delay(1000, () =>
            {
                client.SendToClient(PluginUtils.CreateNotification("Welcome to K Relay!"));
            });
        }

        private void OnUpdate(Client client, UpdatePacket packet)
        {
            client.PlayerData.Parse(packet);
            if (client.State.AccountId != null) return;

            State resolvedState = null;

            foreach (var state in _proxy.States.Values)
                if (state.AccountId == client.PlayerData.AccountId)
                    resolvedState = state;

            if (resolvedState == null)
            {
                client.State.AccountId = client.PlayerData.AccountId;
            }
            else
            {
                foreach (var pair in client.State.States.ToList())
                    resolvedState[pair.Key] = pair.Value;

                foreach (var pair in client.State.States.ToList())
                    resolvedState[pair.Key] = pair.Value;

                client.State = resolvedState;
            }
        }
    }
}
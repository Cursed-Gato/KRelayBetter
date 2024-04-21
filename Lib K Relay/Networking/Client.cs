using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using Lib_K_Relay.Crypto;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking
{
    public class Client
    {
        private static readonly string ClientKey = "5a4d2016bc16dc64883194ffd9";
        private static readonly string ServerKey = "c91d9eec420160730d825604e0";
        private readonly PacketBuffer _clientBuffer = new PacketBuffer();
        private readonly TcpClient _clientConnection;
        private readonly object _clientLock = new object();
        private readonly Rc4Cipher _clientReceiveState = new Rc4Cipher(ClientKey);
        private readonly Rc4Cipher _clientSendState = new Rc4Cipher(ServerKey);
        private readonly NetworkStream _clientStream;
        private readonly Proxy _proxy;
        private readonly PacketBuffer _serverBuffer = new PacketBuffer();
        private readonly object _serverLock = new object();
        private readonly Rc4Cipher _serverReceiveState = new Rc4Cipher(ServerKey);
        private readonly Rc4Cipher _serverSendState = new Rc4Cipher(ClientKey);
        private bool _closed;
        private TcpClient _serverConnection;
        private NetworkStream _serverStream;
        public int LastUpdate = 0;
        public int PreviousTime = 0;
        public int relativeTime = 0;

        public Client(Proxy proxy, TcpClient client)
        {
            _proxy = proxy;
            _clientConnection = client;
            _clientStream = _clientConnection.GetStream();
            _clientConnection.NoDelay = true;
            BeginRead(0, 4, true);
        }

        /// <summary>
        ///     Time since the client's connection began.
        /// </summary>
        public int Time => Environment.TickCount + relativeTime;

        /// <summary>
        ///     Object ID of the client's Player.
        /// </summary>
        public int ObjectId => PlayerData.OwnerObjectId;

        /// <summary>
        ///     PlayerData object of the client's Player.
        /// </summary>
        public PlayerData PlayerData { get; set; }

        /// <summary>
        ///     Account-based state of the client.
        /// </summary>
        public State State { get; set; }

        /// <summary>
        ///     If the client is connected to the client & server.
        /// </summary>
        public bool Connected => !_closed;

        /// <summary>
        ///     Connects the client to the server in the resulting state lookup from the HelloPacket portal key.
        /// </summary>
        /// <param name="state">Packet containing the portal key to be used for the lookup</param>
        public void Connect(HelloPacket state)
        {
            _serverConnection = new TcpClient { NoDelay = true };
            _serverConnection.BeginConnect(State.ConTargetAddress, State.ConTargetPort, ServerConnected, state);
        }

        private void ServerConnected(IAsyncResult ar)
        {
            var success = PluginUtils.ProtectedInvoke(() =>
            {
                _serverConnection.EndConnect(ar);
                _serverStream = _serverConnection.GetStream();
                SendToServer(ar.AsyncState as Packet);
                BeginRead(0, 4, false);
                _proxy.FireClientConnected(this);
                PluginUtils.Log("Client", "Connected to remote host.");
            }, "ClientServerConnect");

            if (!success)
            {
                State.ConTargetAddress = Proxy.DefaultServer;
                State.ConTargetPort = 2050;
                Dispose();
            }
        }

        /// <summary>
        ///     Properly closes and disposes and resources and connections associated with this object.
        /// </summary>
        public void Dispose()
        {
            if (_closed) return;
            _closed = true;
            _proxy.FireClientDisconnected(this);
            _clientStream?.Close();
            _serverStream?.Close();
            _clientConnection?.Close();
            _serverConnection?.Close();
            _clientBuffer.Dispose();
            _serverBuffer.Dispose();
            PluginUtils.Log("Client", "Disconnected.");
        }

        /// <summary>
        ///     Sends a packet to the client.
        /// </summary>
        /// <param name="packet">Packet to be sent</param>
        public void SendToClient(Packet packet)
        {
            Send(packet, true);
        }

        /// <summary>
        ///     Sends a packet to the client's server.
        /// </summary>
        /// <param name="packet">Packet to be sent</param>
        public void SendToServer(Packet packet)
        {
            Send(packet, false);
        }

        private void Send(Packet packet, bool client)
        {
            lock (client ? _clientLock : _serverLock)
            {
                var success = PluginUtils.ProtectedInvoke(() =>
                {
                    var ms = new MemoryStream();
                    using (var w = new PacketWriter(ms))
                    {
                        w.Write(0);
                        w.Write(packet.Id);
                        packet.Write(w);
                        foreach (var b in packet.UnreadData)
                            w.Write(b);
                    }

                    var data = ms.ToArray();
                    PacketWriter.BlockCopyInt32(data, data.Length);

                    if (client)
                    {
                        _clientSendState.Cipher(data);
                        _clientStream.Write(data, 0, data.Length);
                    }
                    else
                    {
                        _serverSendState.Cipher(data);
                        _serverStream.Write(data, 0, data.Length);
                    }
                }, "PacketSend (packet = " + packet?.Type + ")", typeof(IOException));

                if (!success) Dispose();
            }
        }

        private void BeginRead(int offset, int amount, bool client)
        {
            var buffer = client ? _clientBuffer : _serverBuffer;
            var stream = client ? _clientStream : _serverStream;
            stream.BeginRead(buffer.Bytes, offset, amount, RemoteRead,
                new Tuple<NetworkStream, PacketBuffer>(stream, buffer));
        }
        [SuppressMessage("ReSharper.DPA", "DPA0002: Excessive memory allocations in SOH", MessageId = "type: System.String; size: 167MB")]
        private void RemoteRead(IAsyncResult ar)
        {
            var stream = (ar.AsyncState as Tuple<NetworkStream, PacketBuffer>)?.Item1;
            var buffer = (ar.AsyncState as Tuple<NetworkStream, PacketBuffer>)?.Item2;
            var isClient = stream == _clientStream;
            var cipher = isClient ? _clientReceiveState : _serverReceiveState;

            var success = PluginUtils.ProtectedInvoke(() =>
            {
                if (stream != null && !stream.CanRead) return;
                if (buffer == null || stream == null) return;
                var read = stream.EndRead(ar);
                buffer.Advance(read);

                if (read == 0)
                {
                    Dispose();
                }
                else if (buffer.Index == 4)
                {
                    // We have the first four bytes, resize the client buffer
                    buffer.Resize(IPAddress.NetworkToHostOrder(
                        BitConverter.ToInt32(buffer.Bytes, 0)));
                    BeginRead(buffer.Index, buffer.BytesRemaining(), isClient);
                }
                else if (buffer.BytesRemaining() > 0)
                {
                    // Awaiting the rest of the packet
                    BeginRead(buffer.Index, buffer.BytesRemaining(), isClient);
                }
                else
                {
                    // We have the full packet
                    cipher.Cipher(buffer.Bytes);
                    var packet = Packet.Create(buffer.Bytes);

                    if (isClient)
                        _proxy.FireClientPacket(this, packet);
                    else
                        _proxy.FireServerPacket(this, packet);

                    if (packet.Send) Send(packet, !isClient);

                    buffer.Reset();
                    BeginRead(0, 4, isClient);
                }
            }, "RemoteRead (isClient = " + isClient + ")", typeof(IOException));

            if (!success) Dispose();
        }
    }
}
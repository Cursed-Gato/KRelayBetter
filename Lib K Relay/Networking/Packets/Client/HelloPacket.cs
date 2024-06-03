namespace Lib_K_Relay.Networking.Packets.Client
{
    public class HelloPacket : Packet
    {
        public int GameId;
        public string BuildVersion;
        public string AccessToken;
        public int KeyTime;
        public byte[] Key;
        public string GameNet;
        public string PlayPlatform;
        public string PlatformToken;
        public string UserToken;
        public string ClientIdentification;

        public override PacketType Type => PacketType.HELLO;

        public override void Read(PacketReader r)
        {
            GameId = r.ReadInt32();
            BuildVersion = r.ReadString();
            AccessToken = r.ReadString();
            KeyTime = r.ReadInt32();
            Key = r.ReadBytes((int)r.ReadInt16());
            GameNet = r.ReadString();
            PlayPlatform = r.ReadString();
            PlatformToken = r.ReadString();
            UserToken = r.ReadString();
            ClientIdentification = r.ReadString();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(GameId);
            w.Write(BuildVersion);
            w.Write(AccessToken);
            w.Write(KeyTime);
            w.Write((short)Key.Length);
            w.Write(Key);
            w.Write(GameNet);
            w.Write(PlayPlatform);
            w.Write(PlatformToken);
            w.Write(UserToken);
            w.Write(ClientIdentification);
        }
    }
}
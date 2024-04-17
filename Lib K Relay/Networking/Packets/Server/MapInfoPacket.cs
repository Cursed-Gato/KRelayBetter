using Lib_K_Relay.Utilities;
using Org.BouncyCastle.Asn1;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class MapInfoPacket : Packet
    {
        public int Width;
        public int Height;
        public string Name;
        public string DisplayName;
        public string RealmName;
        public int Fp;
        public float Difficulty;
        public int Background;
        public bool AllowPlayerTeleport;
        public bool NoSave;
        public bool ShowDisplays;
        public short MaxPlayers;
        public int GameOpenedTime;
        public string ServerVersion;
        public int BGColor = -1;
        public short unknown = 0;
        public string Modifier = "";
        public int MaxRealmScore = -1;
        public int CurrentRealmScore = 0;

        public override PacketType Type => PacketType.MAPINFO;

        public override void Read(PacketReader r)
        {
            Width = r.ReadInt32();
            Height = r.ReadInt32();
            Name = r.ReadString();
            DisplayName = r.ReadString();
            RealmName = r.ReadString();
            Fp = r.ReadInt32();
            Background = r.ReadInt32();
            Difficulty = r.ReadSingle();
            AllowPlayerTeleport = r.ReadBoolean();
            NoSave = r.ReadBoolean();
            ShowDisplays = r.ReadBoolean();
            MaxPlayers = r.ReadInt16();
            GameOpenedTime = r.ReadInt32();
            ServerVersion = r.ReadString();
            unknown = r.ReadInt16();

            if (r.BaseStream.Position != r.BaseStream.Length)
            {
                BGColor = r.ReadInt32();
            }

            if (r.BaseStream.Position != r.BaseStream.Length)
            {
                Modifier = r.ReadString();
            }

            if (r.BaseStream.Length - r.BaseStream.Position > 7)
            {
                MaxRealmScore = r.ReadInt32();
                CurrentRealmScore = r.ReadInt32();
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Width);
            w.Write(Height);
            w.Write(Name);
            w.Write(DisplayName);
            w.Write(RealmName);
            w.Write(Fp);
            w.Write(Background);
            w.Write(Difficulty);
            w.Write(AllowPlayerTeleport);
            w.Write(NoSave);
            w.Write(ShowDisplays);
            w.Write(MaxPlayers);
            w.Write(GameOpenedTime);
            w.Write(ServerVersion);
            w.Write(unknown);

            if(BGColor != -1)
            {
                w.Write(BGColor);
            }

            if (Modifier != "")
            {
                w.Write(Modifier);
            }

            if (MaxRealmScore != -1)
            {
                w.Write(MaxRealmScore);
                w.Write(CurrentRealmScore);
            }

        }
    }
}
using Lib_K_Relay.Utilities;
using System;
using Newtonsoft.Json.Linq;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class NotificationPacket : Packet
    {
        public NotificationType TypeValue;
        public byte NotificationUnknown;
        public string Message = string.Empty;
        public int ObjectId;
        public int Color;
        public short UiExtra;
        public int QueuePosition;
        public int PictureType;
        public byte TextByte;
        public int UnknownInt1;
        public int UnknownInt2;
        public int UnknownInt3;
        public int UnknownInt4;
        public int UnknownInt5;
        public short UnknownShort;

        public override PacketType Type => PacketType.NOTIFICATION;

        public enum NotificationType : byte
        {
            Player = 0,
            System = 1,
            Error = 2,
            Sticky = 3,
            Global = 4,
            RealmQueue = 5,
            Object = 6,
            PlayerDeath = 7,
            PortalOpened = 8,
            TeleportationError = 9,
            ProgressBar = 10,
            PlayerCallout = 11,
            Behaviour = 12,
            Emote = 13,
            Victory = 14,
            MissionsRefresh = 15,
            MissionProgressOnlyRefresh = 16,
            BlueprintUnlock = 20,
            WithIcon = 21,
            FameBonus = 22,
            ForgeFire = 23
        }

        public override void Read(PacketReader r)
        {
            TypeValue = (NotificationType)r.ReadByte();
            TextByte = r.ReadByte();

            switch (TypeValue)
            {
                case NotificationType.Player:
                case NotificationType.System:
                case NotificationType.Error:
                case NotificationType.Sticky:
                case NotificationType.TeleportationError:
                    Message = r.ReadString();
                    break;
                case NotificationType.Global:
                    Message = r.ReadString();
                    UiExtra = r.ReadInt16();
                    break;
                case NotificationType.RealmQueue:
                    Message = r.ReadString();
                    QueuePosition = r.ReadInt16();
                    break;
                case NotificationType.Object:
                    ReadObjectType(r);
                    break;
                case NotificationType.PlayerDeath:
                case NotificationType.PortalOpened:
                    Message = r.ReadString();
                    PictureType = r.ReadInt32();
                    break;  
                case NotificationType.ProgressBar:
                case NotificationType.PlayerCallout:
                    Message = r.ReadString();
                    UnknownInt1 = r.ReadInt32();
                    UnknownShort = r.ReadInt16();
                    break;
                case NotificationType.Behaviour:
                    Message = r.ReadString();
                    UnknownInt2 = r.ReadInt32();
                    UnknownInt3 = r.ReadInt32();
                    break;
                case NotificationType.Emote:
                    UnknownInt4 = r.ReadInt32();
                    UnknownInt5 = r.ReadInt32();
                    break;
                default:
                    PluginUtils.Log("Notification", $"Received unhandled NotificationType: {TypeValue}!");
                    PluginUtils.Log("Notification", ToString());
                    break;
            }
        }

        private void ReadObjectType(PacketReader reader)
        {
            Message = reader.ReadString();
            if (TextByte != 6) return; // Portal opened

            // example message: {"k":"s.opened_by","t":{"player":"IlliIIlIlllIilI",}}
            Message = Message.Replace(",}", "}");
            var jsonObject = JObject.Parse(Message);

            var kValue = jsonObject["k"].Value<string>();
            var tValue = jsonObject["t"].Value<JObject>();
            var playerValue = tValue["player"].Value<string>();

            ObjectId = reader.ReadInt32();
            Color = reader.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write((byte)TypeValue);
            w.Write(TextByte);

            switch (TypeValue)
            {
                case NotificationType.Player:
                case NotificationType.System:
                case NotificationType.Error:
                case NotificationType.Sticky:
                case NotificationType.TeleportationError:
                    w.Write(Message);
                    break;
                case NotificationType.Global:
                    w.Write(Message);
                    w .Write(UiExtra);
                    break;
                case NotificationType.RealmQueue:
                    w.Write(Message);
                    w.Write(QueuePosition);
                    break;
                case NotificationType.Object:
                    w.Write(Message);
                    w.Write(ObjectId);
                    w.Write(Color);
                    break;
                case NotificationType.PlayerDeath:
                case NotificationType.PortalOpened:
                    w.Write(Message);
                    w.Write(PictureType);
                    break;
                case NotificationType.ProgressBar:
                case NotificationType.PlayerCallout:
                    w.Write(Message);
                    w.Write(UnknownInt1);
                    w   .Write(UnknownShort);
                    break;
                case NotificationType.Behaviour:
                    w.Write(Message);
                    w.Write(UnknownInt2);
                    w.Write(UnknownInt3);
                    break;
                case NotificationType.Emote:
                    w.Write(UnknownInt4);
                    w.Write(UnknownInt5);
                    break;
                default:
                    PluginUtils.Log("Notification", $"Attempted to write unknown NotificationType: {TypeValue}!");
                    PluginUtils.Log("Notification", ToString());
                    break;
            }
        }
    }
}
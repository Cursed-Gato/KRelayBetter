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
		public short QueuePosition;
		public int PictureType;
		public byte TextByte;
		public int SenderObjectId;
		public int UnknownInt2;
		public int UnknownInt3;
		public int UnknownInt4;
		public int EmoteType;
        public int RealmQueueMessageType;
		public int ProgressMax;
        public short ProgressValue;
        public short NumStars;

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
			PlayerCallout = 10,
			ProgressBar = 11,
			Behaviour = 12,
			Emote = 13,
			Victory = 14,
            MissionsRefresh = 15,
            CrucibleDataRefresh = 16,
			BlueprintUnlock = 20,
			WithIcon = 21,
			FameBonus = 22,
			ForgeFire = 23,
            MissionsProgressOnlyRefresh = 99
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
				case NotificationType.TeleportationError:
					Message = r.ReadString();
					break;
                case NotificationType.Sticky:
                    Message = r.ReadString();
					break;
                case NotificationType.Global:
					Message = r.ReadString();
					UiExtra = r.ReadInt16();
					break;
				case NotificationType.RealmQueue:
                    RealmQueueMessageType = r.ReadInt32();
					QueuePosition = r.ReadInt16();
					break;
				case NotificationType.Object:
					ReadObjectType(r);
					break;
				case NotificationType.PlayerDeath:
                    Message = r.ReadString();
                    PictureType = r.ReadInt32();
                    break;
				case NotificationType.PortalOpened:
					Message = r.ReadString();
					PictureType = r.ReadInt32();
					break;  
				case NotificationType.PlayerCallout:
					Message = r.ReadString();
					SenderObjectId = r.ReadInt32();
					NumStars = r.ReadInt16();
					break;
				case NotificationType.ProgressBar:
					if (TextByte == 0)
						break;

					if((TextByte & 3) != 0)
					{
                        Message = r.ReadString();
                    }
					ProgressMax = r.ReadInt32();
					ProgressValue = r.ReadInt16();
					break;
				case NotificationType.Behaviour:
					Message = r.ReadString();
					PictureType = r.ReadInt32();
                    Color = r.ReadInt32();
					break;
				case NotificationType.Emote:
                    ObjectId = r.ReadInt32();
                    EmoteType = r.ReadInt32();
					break;
                case NotificationType.Victory:
                case NotificationType.MissionsRefresh:
				case NotificationType.CrucibleDataRefresh:
                case NotificationType.MissionsProgressOnlyRefresh:
                    break;
                default:
					PluginUtils.Log("Notification", $"Received unhandled NotificationType: {TypeValue}!");
					PluginUtils.Log("Notification", ToString());
					break;
			}
		}

		private void ReadObjectType(PacketReader r)
		{
			Message = r.ReadString();

			// example message: {"k":"s.opened_by","t":{"player":"IlliIIlIlllIilI",}}
			/*Message = Message.Replace(",}", "}");
			var jsonObject = JObject.Parse(Message);

			var kValue = jsonObject["k"].Value<string>();
			var tValue = jsonObject["t"].Value<JObject>();
			var playerValue = tValue["player"].Value<string>();*/

			ObjectId = r.ReadInt32();
			Color = r.ReadInt32();
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
                case NotificationType.TeleportationError:
                case NotificationType.Sticky:
                    w.Write(Message);
                    break;
                case NotificationType.Global:
                    w.Write(Message);
                    w.Write(UiExtra);
                    break;
                case NotificationType.RealmQueue:
                    w.Write(RealmQueueMessageType);
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
                case NotificationType.PlayerCallout:
                    w.Write(Message);
                    w.Write(SenderObjectId);
                    w.Write(NumStars);
                    break;
                case NotificationType.ProgressBar:
                    if ((TextByte & 3) != 0)
                    {
                        w.Write(Message);
                    }
                    w.Write(ProgressMax);
                    w.Write(ProgressValue);
                    break;
                case NotificationType.Behaviour:
                    w.Write(Message);
                    w.Write(PictureType);
                    w.Write(Color);
                    break;
                case NotificationType.Emote:
                    w.Write(ObjectId);
                    w.Write(EmoteType);
                    break;
                case NotificationType.Victory:
                case NotificationType.MissionsRefresh:
                case NotificationType.CrucibleDataRefresh:
                case NotificationType.MissionsProgressOnlyRefresh:
                    break;
                default:
                    PluginUtils.Log("Notification", $"Attempted to write unknown NotificationType: {TypeValue}!");
                    PluginUtils.Log("Notification", ToString());
                    break;
            }
        }
    }
}
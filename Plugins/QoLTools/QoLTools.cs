using Lib_K_Relay;
using Lib_K_Relay.GameData;
using Lib_K_Relay.Interface;
using Lib_K_Relay.Networking;
using Lib_K_Relay.Networking.Packets;
using Lib_K_Relay.Networking.Packets.Client;
using Lib_K_Relay.Networking.Packets.DataObjects;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QoLTools
{
    public class QoLTools : IPlugin
	{

        private static readonly HttpClient client = new HttpClient();
        private List<byte> SkipPacketLog;
		private List<byte> PacketsToTrack;
        private List<byte> PacketsToIgnore;
        private Dictionary<int, Packet> CreatedPackets;
		private Dictionary<int, List<Packet>> PacketsToSendOn;
		private int PacketCreaterIt = 0;
		private bool LogPackets = false;
		private bool LogToClient = false;
		private bool ChestMode = false;
		private bool AbilitySpamEnabled = false;

		UseItemPacket LastUseItem = null;
        ReconnectPacket lastReconPacket = null;
        ClaimChestRewardPacket LastChestClaim = null;
		private static HelloPacket LastHello = null;

        private bool createChar = false;
        private bool changeGameId = false;
        private bool changeLoad = false;
        private bool doubleConnect = false;
        int loadId = 0;
        int gameId = 0;

        public string GetAuthor()
		{
			return "smol";
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
				"/track remove:add:reset",
				"/chestmode",
				"/useall {itemid}",
				"/inv",
				"/packet create:reset:send:name:id:sendm:edit",
			};
		}

		public void Initialize(Proxy proxy)
		{
			SkipPacketLog = new List<byte>();
			PacketsToIgnore = new List<byte>();
			CreatedPackets = new Dictionary<int, Packet>();
			PacketsToSendOn = new Dictionary<int, List<Packet>>();
			PacketsToTrack = new List<byte>();

			ServicePointManager.DefaultConnectionLimit = int.MaxValue;

            proxy.ServerPacketRecieved += OnServerPacket;
			proxy.ClientPacketRecieved += OnClientPacket;
			proxy.HookCommand("logpackets", OnLoggerCommamd);
			proxy.HookCommand("track", onTrackCommand);
			proxy.HookCommand("useall", onUseAllCommand);
			proxy.HookCommand("ability", onAbilitySpamCommand);
            proxy.HookCommand("dupey", (Client client, string cmd, string[] args) =>
			{
				foreach (var packetClass in GameData.Packets.Map)
                {
					try
                    {
                        var packetInstance = Packet.Create(GameData.Packets.ById(packetClass.Value.Id).PacketType);
                        PluginUtils.Log("Packet", packetInstance.Id + " - >" + packetInstance.GetType().Name);

                    }
					catch
					{

					}

                }
            });
            proxy.HookCommand("gameid", (Client client, string cmd, string[] args) =>
            {
                changeGameId = !changeGameId;
                gameId = args[0].ParseInt();

            });
            proxy.HookCommand("load", (Client client, string cmd, string[] args) =>
            {
				if(args.Length == 0)
				{
					doubleConnect = !doubleConnect;
                    return;
				}
				loadId = args[0].ParseInt();
                changeLoad = !changeLoad;
            });
            proxy.HookCommand("spam", onSpamCommand);
			proxy.HookCommand("message", (Client client, string cmd, string[] args) =>
			{
				var toSend = args[0];
				string capitalizedStr = toSend.Substring(0, 1).ToUpper() + toSend.Substring(1);

				var finalMessage = "/tell " + capitalizedStr + " 23\r\n\t23";
				var playerTextPacket = Packet.Create<PlayerTextPacket>(PacketType.PLAYERTEXT);
				playerTextPacket.Text = finalMessage;
				client.SendToServer(playerTextPacket);
			});
			proxy.HookCommand("inv", onInvCommand);
			proxy.HookCommand("chestmode", (Client client, string cmd, string[] args) => { ChestMode = !ChestMode; });
			proxy.HookPacket<UseItemPacket>(onUseItem);
			proxy.HookPacket((Client client, PlayerShootPacket packet) => { if (AbilitySpamEnabled) packet.Send = false; });
            proxy.HookPacket((Client client, LoadPacket packet) => 
			{
				if (createChar)
				{
					packet.Send = false;
					var createPacket = Packet.Create<CreatePacket>(PacketType.CREATE);
					createPacket.ClassType = 782;
					createPacket.FirstSession = false;
					createPacket.SeasonalChar = false;
					createPacket.SkinType = 0;

					client.SendToServer(createPacket);
					createChar = false;
                }
				if (changeLoad)
				{
					if(loadId != 0)
                    {
                        packet.CharacterId = loadId;
                    }
					changeLoad = false;
				}
				if (doubleConnect)
				{
					client.SendToClient(lastReconPacket);
					doubleConnect = false;
                }
			}); 
			proxy.HookPacket((Client client, HelloPacket packet) =>
            {
                if (changeGameId)
                {
					packet.GameId = gameId;
                    changeGameId = false;
                }
            });
            proxy.HookPacket((Client client, ReconnectPacket packet) =>
            {
				lastReconPacket = packet;
            });
            proxy.HookPacket<ClaimRewardsInfoPrompt>(onCliamRewardsInfoItem);
			proxy.HookPacket<ClaimChestRewardPacket>(onCliamRewards);
			proxy.HookPacket<ChestRewardResultPacket>(onChestResultRewards);
			proxy.HookPacket((Client client, HelloPacket packet) => { LastHello = packet; });
			proxy.HookCommand("packet", onPacketCommand);
		}
		public static string Encode(string input)
		{
			var stringBuilder = new StringBuilder();
			foreach (var c in input)
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append('%');
					stringBuilder.Append(((byte)c).ToString("X2"));
				}

			return stringBuilder.ToString();
		}

        public static string CharDelete(string charId)
        {
            var encodedAccessToken = Encode(LastHello.AccessToken);

            using (var webClient = new WebClient())
            {
                var postData = $"charId={charId}&amp;reason=2&amp;accessToken={encodedAccessToken}&amp;game_net=Unity&amp;play_platform=Unity&amp;game_net_user_id=";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    return webClient.UploadString("https://www.realmofthemadgod.com/char/delete", postData);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)ex.Response)
                        using (var reader = new System.IO.StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            return errorText;
                        }
                    }
                    else
                    {
                        return ex.Message;
                    }
                }
            }
        }

        public static string GetPetSkinOwned()
        {
            var encodedAccessToken = Encode(LastHello.AccessToken);

            using (var webClient = new WebClient())
            {
                var postData = $"accessToken={encodedAccessToken}&amp;game_net=Unity&amp;play_platform=Unity&amp;game_net_user_id=";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    return webClient.UploadString("https://www.realmofthemadgod.com/account/getOwnedPetSkins", postData);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)ex.Response)
                        using (var reader = new System.IO.StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            return errorText;
                        }
                    }
                    else
                    {
                        return ex.Message;
                    }
                }
            }
        }

        public static string FetchCalander()
        {
            var encodedAccessToken = Encode(LastHello.AccessToken);

            using (var webClient = new WebClient())
            {
                var postData = $"accessToken={encodedAccessToken}&amp;game_net=Unity&amp;play_platform=Unity&amp;game_net_user_id=";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    return webClient.UploadString("https://www.realmofthemadgod.com/dailyLogin/fetchCalendar", postData);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)ex.Response)
                        using (var reader = new System.IO.StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            return errorText;
                        }
                    }
                    else
                    {
                        return ex.Message;
                    }
                }
            }
        }

        public static string ListPowerUp()
        {
            var encodedAccessToken = Encode(LastHello.AccessToken);

            using (var webClient = new WebClient())
            {
                var postData = $"accessToken={encodedAccessToken}&amp;game_net=Unity&amp;play_platform=Unity&amp;game_net_user_id=";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";

                try
                {
                    return webClient.UploadString("https://www.realmofthemadgod.com/account/listPowerUpStats", postData);
                }
                catch (WebException ex)
                {
                    if (ex.Response != null)
                    {
                        using (var errorResponse = (HttpWebResponse)ex.Response)
                        using (var reader = new System.IO.StreamReader(errorResponse.GetResponseStream()))
                        {
                            string errorText = reader.ReadToEnd();
                            return errorText;
                        }
                    }
                    else
                    {
                        return ex.Message;
                    }
                }
            }
        }

        void onUseItem(Client client, UseItemPacket packet)
		{
			LastUseItem = packet;

			if (AbilitySpamEnabled)
			{
				if (packet.UseType == 1)
				{
					packet.UseType = 0;
				}
				if (packet.UseType == 2)
				{
					packet.UseType = 0;
				}
				/*if (packet.UseType == 1)
				{
					for (int i = 0; i < AbilitySpamCount; i++)
					{
						packet.Time += (i * 550);
						packet.UseType = (byte)((i % 2) == 0 ? 1 : 2);
						client.SendToServer(packet);
					}
					client.SendToClient(PluginUtils.CreateOryxNotification("QOL", "Ability Spammed " + AbilitySpamCount.ToString() + " Times!"));
					//DirectNexus(client);

					packet.Send = false;
				}
				else if (packet.UseType == 2)
				{
					DirectNexus(client);
					packet.Send = false;
				}*/
			}
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

		public void onSpamCommand(Client client, string cmd, string[] args)
		{
			if(args.Length < 1)
			{
				client.SendToClient(PluginUtils.CreateOryxNotification("AbilitySpam", "Incorecct Usage, first int amount"));
				return;
			}
			int times;
			if (int.TryParse(args[0], out times))
			{
                var tasks = new List<Task<string>>();

                for (int i = 0; i < times; i++)
				{
                    Thread thread = new Thread( () =>
                    {
                        var result = i % 2 == 0 ?  FetchCalander() : FetchCalander();
                    });
					thread.Start();
				}
            }

			client.SendToClient(PluginUtils.CreateOryxNotification("AbilitySpam", $"Spammed Char Delete on char {args[0]} times!"));
		}

		public void onAbilitySpamCommand(Client client, string cmd, string[] args)
		{
			var subCommand = args[0];
			if (subCommand == "on")
			{
				AbilitySpamEnabled = true;
				client.SendToClient(PluginUtils.CreateOryxNotification("AbilitySpam", "Enabled"));
				return;
			}
			if (subCommand == "off")
			{
				AbilitySpamEnabled = false;
				client.SendToClient(PluginUtils.CreateOryxNotification("AbilitySpam", "Disabled"));
				return;
			}
			if (subCommand == "amount")
			{
				//AbilitySpamCount = args[1].ParseInt();
				client.SendToClient(PluginUtils.CreateOryxNotification("AbilitySpam", "Spamming " + args[1] + " Times!"));
				return;
			}


			client.SendToClient(PluginUtils.CreateOryxNotification("AbilitySpam", "Incorrect Usage!!!"));
			return;
		}

		public void onPacketCommand(Client client, string cmd, string[] args)
		{
			if (args[0] == "create")
			{
				var parsedArgs = ParseArgs(args);
				PacketType packetType = GetPacketType(parsedArgs[1]);
				var workingPacket = Packet.Create(packetType);
				var fields = workingPacket.GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

				//Check if there are enough feilds given to create the packet
				if (fields.Length != parsedArgs.Length + 1)
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", (parsedArgs.Length - 2).ToString() + " given, but " + (fields.Length - 3).ToString() + " needed!"));
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", workingPacket.ToStructure()));
					return;
				}

				//need to parse the fields in question
				int fieldsToParse = fields.Length - 3;
				for (int i = 0; i < fieldsToParse; i++)
				{
					var fieldType = fields[i].FieldType;
					object fieldValue = parsedArgs[i + 2];
					object parsedValue = ParseFieldValue(client, fieldType, fieldValue);
					if(parsedValue == null)
					{
						client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "failed to parse " + fieldType.Name + ", given " + parsedArgs[i + 2]));
						return;
					}

					fields[i].SetValue(workingPacket, parsedValue);
				}

				CreatedPackets.Add(PacketCreaterIt, workingPacket);
				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", PacketCreaterIt.ToString() + " -> " + workingPacket.ToString()));
				PacketCreaterIt++;
				return;
			}

			if (args[0] == "reset")
			{
				PacketCreaterIt = 0;
				CreatedPackets.Clear();
				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Reset Packets"));
				return;
			}

            if (args[0] == "ignore")
            {
				if (args.Length < 2)
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Incorrect Usage, /packet ignore reset:{packet}"));

                }

				if (args[1] == "reset")
				{
					PacketsToIgnore.Clear();
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Reset Ignored Packets"));
                    return;
				}

				PacketType packetType = GetPacketType(args[1]);
                var packetToignore = Packet.Create(packetType);

                if (!PacketsToIgnore.Contains(packetToignore.Id))
                {
                    PacketsToIgnore.Add(packetToignore.Id);
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Now Ignoring " + packetToignore.Id.ToString() + "!"));
                }
				else
                {
                    client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Already Ignoring!"));
                }

                return;
            }

            if (args[0] == "sendon")
			{
				PacketType packetType = GetPacketType(args[1]);
				var packetToSendOn = Packet.Create(packetType);

				Packet packetToSend;
				if (!CreatedPackets.TryGetValue(args[2].ParseInt(), out packetToSend))
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Packet Iterator is not Valid!"));
					return;
				}

				if (PacketsToSendOn.ContainsKey(packetToSendOn.Id))
				{
					PacketsToSendOn[packetToSendOn.Id].Add(packetToSend);
				}
				else
				{
					PacketsToSendOn[packetToSendOn.Id] = new List<Packet> { packetToSend };
				}

				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Registered packet send on " + packetType.ToString()));
				return;
			}

			if (args[0] == "send")
			{
				if (args.Length < 3)
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Invalid Usage, /packet send client:server {PacketIt}"));
					return;
				}

				Packet packetToSend;
				if (!CreatedPackets.TryGetValue(args[2].ParseInt(), out packetToSend))
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Packet Iterator is not Valid!"));
					return;
				}

				if (args[1].ToLower() == "client")
				{
					client.SendToClient(packetToSend);
				}
				else if (args[1].ToLower() == "server")
				{
					if (packetToSend.GetType().GetField("Time") != null)
					{
						var clientTime = client.Time;
						packetToSend.GetType().GetField("Time").SetValue(packetToSend, clientTime);
					}
					client.SendToServer(packetToSend);
				}
				else
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Invalid Usage, needs to be either server or client!"));
					return;
				}

				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Sent Packet!"));
				return;
			}

			if (args[0] == "sendm")
			{
				if (args.Length < 4)
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Invalid Usage, /packet send client:server {TimesToSend} {PacketIt}"));
					return;
				}

				Packet packetToSend;
				if (!CreatedPackets.TryGetValue(args[3].ParseInt(), out packetToSend))
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Packet Iterator is not Valid, usage /packet send client:server {TimesToSend} {PacketIt}!"));
					return;
				}

				if (args[1].ToLower() == "client")
				{
					for (int i = 0; i < args[2].ParseInt(); i++)
						client.SendToClient(packetToSend);
				}
				else if (args[1].ToLower() == "server")
				{
					for (int i = 0; i < args[2].ParseInt(); i++)
						client.SendToServer(packetToSend);
				}
				else
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Invalid Usage, needs to be either server or client!"));
					return;
				}

				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Sent " + args[2] + " Packets!"));
				return;
			}

			if (args[0] == "name")
			{
				string keyword = args[1].ToLower();

				var matchingPackets = GameData.Packets.Map
					.Where(packetTypeEntry => packetTypeEntry.Value.Name.ToLower().Contains(keyword))
					.Select(packetTypeEntry => $"{packetTypeEntry.Value.Name} -> {packetTypeEntry.Key}");

				string result = string.Join("\n", matchingPackets);

				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", result));
				return;
			}

			if (args[0] == "id")
			{
				var packet = Packet.Create(GameData.Packets.ById((byte)args[1].ParseInt()).PacketType);
				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", args[1] + " -> " + packet.ToStructure()));
				return;
			}

			if (args[0] == "edit")
			{
				if (args.Length < 4)
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "Incorrect Usage, /packet edit {packetIt} {fieldName} {value}"));
					return;
				}

				if (!CreatedPackets.ContainsKey(args[1].ParseInt()))
				{
					client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", "No Packet Iterator associated with " + args[1]));
					return;
				}

				var packetToEdit = CreatedPackets[args[1].ParseInt()];

				var packetCopy = Packet.Create(packetToEdit.Type);

				foreach (var field in packetToEdit.GetType().GetFields())
				{
					if (field.Name.ToLower() == args[2].ToLower())
					{
						var valueToSet = ParseFieldValue(client, field.FieldType, args[3]);

						if(valueToSet == null)
						{
							client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", args[3] + " Failed to Parse!"));
							return;
						}
						field.SetValue(packetCopy, valueToSet);
					}
					else
						field.SetValue(packetCopy, field.GetValue(packetToEdit));
				}

				CreatedPackets.Add(PacketCreaterIt, packetCopy);
				client.SendToClient(PluginUtils.CreateOryxNotification("Packeteer", PacketCreaterIt.ToString() + " -> " + packetCopy.ToString()));
				PacketCreaterIt++;
			}
		}

		private string[] ParseArgs(string[] args)
		{
			List<string> parsedArgs = new List<string>();
			bool insideQuotes = false;
			string concatenatedArg = "";

			foreach (string arg in args)
			{
				if (insideQuotes)
				{
					// If inside quotes, concatenate the argument
					concatenatedArg += arg + " ";
					if (concatenatedArg.EndsWith("\' "))
					{
						// If this argument ends with a quote, remove the trailing quote
						concatenatedArg = concatenatedArg.TrimEnd('\'', ' ');
						parsedArgs.Add(concatenatedArg);
						insideQuotes = false;
						concatenatedArg = "";
					}
				}
				else
				{
					if (arg.StartsWith("\'"))
					{
						// If the argument starts with a quote, start concatenating arguments
						insideQuotes = true;
						concatenatedArg = arg.Substring(1) + " "; // Remove the leading quote
					}
					else
					{
						parsedArgs.Add(arg);
					}
				}
			}

			return parsedArgs.ToArray();
		}

		private static object ParseArg(object client, string arg)
		{
			//This Function will take an object and get the value from it;
			//so client.ObjectId wil return the value in client.ObjectId {ex: 19834} and return it as a string;
			//this function will also check wiht ignored case, if not found returns null 

			if (!arg.Contains("."))
			{
				return arg;
			}

			var currentObject = client;
			var fieldsOrdered = arg.Split('.').ToArray();

			for (int i = 1; i < fieldsOrdered.Length; i++)
			{
				var field = fieldsOrdered[i];
				var fieldInfo = currentObject.GetType().GetField(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
				if (fieldInfo != null)
					currentObject = fieldInfo.GetValue(currentObject);
				else
				{
					// Check properties if field is not found
					var propertyInfo = currentObject.GetType().GetProperty(field, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
					if (propertyInfo != null)
					{
						currentObject = propertyInfo.GetValue(currentObject);
					}
					else
					{
						// Neither field nor property found, return null
						return null;
					}
				}
			}

			if (currentObject == null)
				return null;
			else
				return currentObject;
		}

		private object ParseFieldValue(Client client, Type fieldType, object fieldValue)
		{
			// Implement parsing logic based on field type
			if (fieldType.IsArray)
			{
				Type elementType = fieldType.GetElementType();

				if (elementType.IsClass && elementType != typeof(string))
				{
					string[] objectStrings = fieldValue.ToString().TrimStart('{').TrimEnd('}').Split(new string[] { "},{" }, StringSplitOptions.None);

					Array array = Array.CreateInstance(elementType, objectStrings.Length);

					for (int i = 0; i < objectStrings.Length; i++)
					{
						array.SetValue(ParseFieldValue(client, elementType, "{" + objectStrings[i] + "}"), i);
					}

					return array;
				}
				else
				{
					string[] values = fieldValue.ToString().Split(',');

					Array array = Array.CreateInstance(elementType, values.Length);

					for (int i = 0; i < values.Length; i++)
					{
						array.SetValue(ParseFieldValue(client, elementType, values[i]), i);
					}

					return array;
				}
			}

			if (!fieldValue.ToString().StartsWith("{"))
				fieldValue = ParseArg(client, fieldValue.ToString());

			if (fieldType == typeof(int))
			{
				int parsedInt;
				if (int.TryParse(fieldValue.ToString(), out parsedInt))
				{
					return parsedInt;
				}
				return null;
			}
			else if (fieldType == typeof(byte))
			{
				byte parsedByte;
				if (byte.TryParse(fieldValue.ToString(), out parsedByte))
				{
					return parsedByte;
				}
				return null;
			}
			else if (fieldType == typeof(float))
			{
				float parsedFloat;
				if (float.TryParse(fieldValue.ToString(), out parsedFloat))
				{
					return parsedFloat;
				}
				return null;
			}
			else if (fieldType == typeof(bool))
			{
				bool parsedBool;
				if (bool.TryParse(fieldValue.ToString(), out parsedBool))
				{
					return parsedBool;
				}
				return null;
			}
			else if (fieldType == typeof(short))
			{
				short parsedShort;
				if (short.TryParse(fieldValue.ToString(), out parsedShort))
				{
					return parsedShort;
				}
				return null;
			}
			else if (fieldType == typeof(sbyte))
			{
				sbyte parsedSByte;
				if (sbyte.TryParse(fieldValue.ToString(), out parsedSByte))
				{
					return parsedSByte;
				}
				return null;
			}
			else if (fieldType == typeof(string))
			{
				return fieldValue;
			}
			else if (fieldType == typeof(uint))
			{
				uint parsedUInt;
				if (uint.TryParse(fieldValue.ToString(), out parsedUInt))
				{
					return parsedUInt;
				}
				return null;
			}
			else if (fieldType == typeof(ushort))
			{
				ushort parsedUShort;
				if (ushort.TryParse(fieldValue.ToString(), out parsedUShort))
				{
					return parsedUShort;
				}
				return null;
			}
			else
			{
				//if not any predetermined it must be custom object
				if(fieldValue.GetType() == typeof(string))
				{
					string[] fieldValues = fieldValue.ToString().Trim('{', '}').Split(',');
					object customObject = Activator.CreateInstance(fieldType);

					for (int i = 0; i < fieldValues.Length; i++)
					{
						FieldInfo field = fieldType.GetFields()[i];
						field.SetValue(customObject, ParseFieldValue(client, field.FieldType, fieldValues[i]));
					}

					return customObject;
				}
				if (fieldValue.GetType() != fieldType)
				{
					return null;
				}

				return fieldValue;
			}
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

			if(args.Length < 2)
			{
				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Incorrect Usage, /track add:remove {name/id} or /track reset!"));
				return;
			}

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
					client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Failed to get Packet, packetId is incorrect!"));
				}
				else
				{
					if (args[0].ToLower() == "add")
					{
						if (!PacketsToTrack.Contains(packetId))
						{
							PacketsToTrack.Add(packetId);
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Added id " + packetId.ToString() + "!"));
						}
						else
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Already Added!"));
					}
					else
					{
						if (PacketsToTrack.Contains(packetId))
						{
							PacketsToTrack.Remove(packetId);
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Removed id " + packetId.ToString() + "!"));
						}
						else
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", "Already Removed!"));
					}
				}
				return;
			}

			if (args[0].ToLower() == "reset")
			{
				PacketsToTrack.Clear();
				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Reset Tracked Packets!"));
				return;
			}

			client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Incorrect Usage, use add:remove:reset!"));
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
						{
							SkipPacketLog.Add(packetId);
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Added id " + packetId.ToString() + "!"));
						}
						else
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Already Added!"));
					}
					else
					{
						if (SkipPacketLog.Contains(packetId))
						{
							SkipPacketLog.Remove(packetId);
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Removed id " + packetId.ToString() + "!"));
						}
						else
							client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Already Removed!"));
					}
				}
				return;
			}

			if (args[0].ToLower() == "start")
			{
				LogPackets = true;
				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Started Logging!"));
				return;

			}
			if (args[0].ToLower() == "stop")
			{
				LogPackets = false;
				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Stopped Logging!"));
				return;
			}
			if (args[0].ToLower() == "reset")
			{
				SkipPacketLog.Clear();
				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Reset Packet to Log!"));
				return;
			}
			if (args[0].ToLower() == "toclient")
			{
				LogToClient = !LogToClient;
				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", LogToClient ? "Logging Packets to Client!" : "Logging Packets to Gui!"));
				return;
			}
			if (args[0].ToLower() == "limited")
			{
				string[] packets = { "hello", "move", "load", "mapinfo", "update", "updateack", "newtick", "ping", "pong", "notification", "text", "showeffect" };
				foreach (var packet in packets)
				{
					SkipPacketLog.Add(GameData.Packets.ByName(packet.ToUpper()).Id);
				}


				client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Registered Packets!"));

				return;
			}


			client.SendToClient(PluginUtils.CreateOryxNotification("Packet Logger", "Incorerect Usage, /logpackets limited:remove:add:stop:start:reset:toclient!"));
			return;
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
			if (PacketsToSendOn.ContainsKey(packet.Id))
			{
				var packetsToSend = PacketsToSendOn[packet.Id];

				foreach(var packetToSend in packetsToSend)
				{
					client.SendToServer(packetToSend);
				}
			}

			if (!PacketsToTrack.Contains(packet.Id))
				return;

			CreatedPackets.Add(PacketCreaterIt, packet);
			client.SendToClient(PluginUtils.CreateOryxNotification("Packet Tracker", PacketCreaterIt.ToString() + " -> " + packet.Type.ToString()));
			PacketCreaterIt++;

		}

		public void OnServerPacket(Client client, Packet packet)
		{
			if (PacketsToIgnore.Contains(packet.Id))
			{
				packet.Send = false;
				return;
			}

			LogPacket(client, packet);
		}
		
		public void OnClientPacket(Client client, Packet packet)
		{
            if (PacketsToIgnore.Contains(packet.Id))
            {
                packet.Send = false;
                return;
            }

            LogPacket(client, packet);
			TrackPacket(client, packet);
		}

		public PacketType GetPacketType(string packetName)
		{
			//This can be a name or an either way returns the id
			byte idToReturn = 255;
			if(!byte.TryParse(packetName, out idToReturn))
			{
				return GameData.Packets.ByName(packetName.ToUpper()).PacketType;
			}
			else
			{
				return GameData.Packets.ById(idToReturn).PacketType;
			}
		}

		private void DirectNexus(Client client)
		{
			var nexusRecon = (ReconnectPacket)Packet.Create(PacketType.RECONNECT);
			nexusRecon.Host = "";
			nexusRecon.Port = (ushort)2050;
			nexusRecon.Name = "Nexus";
			nexusRecon.Key = new byte[0];
			nexusRecon.KeyTime = 0;
			nexusRecon.GameId = -2;
			client.SendToClient(nexusRecon);
			client.SendToServer(Packet.Create(PacketType.ESCAPE));
		}
	}
}
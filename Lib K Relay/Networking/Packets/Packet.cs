using System;
using System.IO;
using System.Reflection;
using System.Text;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets
{
    public class Packet
    {
        private byte[] _data;
        public byte Id;
        public bool Send = true;
        public byte[] UnreadData = new byte[0];

        public virtual PacketType Type => PacketType.UNKNOWN;

        public virtual void Read(PacketReader r)
        {
            _data = r.ReadBytes((int)r.BaseStream.Length - 5); // All of the packet data
        }

        public virtual void Write(PacketWriter w)
        {
            w.Write(_data); // All of the packet data
        }

        public static Packet Create(PacketType type)
        {
            var st = GameData.GameData.Packets.ByName(type.ToString());
            var packet = (Packet)Activator.CreateInstance(st.Type);
            packet.Id = st.Id;
            return packet;
        }

        public static T Create<T>(PacketType type)
        {
            var packet = (Packet)Activator.CreateInstance(typeof(T));
            packet.Id = GameData.GameData.Packets.ByName(type.ToString()).Id;
            return (T)Convert.ChangeType(packet, typeof(T));
        }

        public T To<T>()
        {
            return (T)Convert.ChangeType(this, typeof(T));
        }

        public static Packet Create(byte[] data)
        {
            using (var r = new PacketReader(new MemoryStream(data)))
            {
                r.ReadInt32(); // Skip over int length
                var id = r.ReadByte();

                // 254 = We don't have the packet defined, log data and send back
                var st = GameData.GameData.Packets.ById(
                    !GameData.GameData.Packets.Map.ContainsKey(id) ? (byte)254 : id);
                var type = st.Type;

                // Reflect the type to a new instance and read its data from the PacketReader
                
                if(type == null)
                {
                    st = GameData.GameData.Packets.ById(254);
                    type = st.Type;
                }
                var packet = (Packet)Activator.CreateInstance(type);
                packet.Id = id;
                packet.Read(r);

                // Handle all unprocessed bytes in order to ensure packet integrity
                if (r.BaseStream.Position != r.BaseStream.Length)
                {
                    var len = r.BaseStream.Length - r.BaseStream.Position;
                    packet.UnreadData = new byte[len];
                    var msg = "Packet has unread data left over: " +
                              "Id=" + packet.Id + ", Data=[";
                    for (var i = 0; i < len; i++)
                    {
                        packet.UnreadData[i] = r.ReadByte();
                        msg += packet.UnreadData[i] + (i == len - 1 ? "]" : ",");
                    }

                    PluginUtils.Log("Packet", msg);
                }
                return packet;
            }
        }

        public override string ToString()
        {
            // Use reflection to get the packet's fields and values so we don't have
            // to formulate a ToString method for every packet type.
            var fields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(Type.ToString() + "(" + Id.ToString() + ") Packet Instance");
            foreach (var fieldInfo in fields)
                stringBuilder.Append("      " + fieldInfo.Name + " => " + fieldInfo.GetValue(this)?.ToString());
            return stringBuilder.ToString();
        }

        public string ToStructure()
        {
            // Use reflection to build a list of the packet's fields.
            var fields = GetType().GetFields(BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance);

            var s = new StringBuilder();
            s.Append(Type + " [" + GameData.GameData.Packets.ByName(Type.ToString()).Id + "] \nPacket Structure:\n{");
            foreach (var f in fields) s.Append("\n  " + f.Name + " => " + f.FieldType.Name);

            s.Append("\n}");
            return s.ToString();
        }
    }

    public enum PacketType
    {
        FAILURE,
        TELEPORT,
        CLAIMDAILYREWARD,
        DELETEPET,
        REQUESTTRADE,
        QUESTFETCHRESPONSE,
        JOINGUILD,
        PING,
        PLAYERTEXT,
        NEWTICK,
        SHOWEFFECT,
        SERVERPLAYERSHOOT,
        USEITEM,
        TRADEACCEPTED,
        GUILDREMOVE,
        PETUPGRADEREQUEST,
        GOTO,
        INVENTORYDROP,
        OTHERHIT,
        NAMERESULT,
        BUYRESULT,
        HATCHPET,
        ACTIVEPETUPDATEREQUEST,
        ENEMYHIT,
        GUILDRESULT,
        EDITACCOUNTLIST,
        TRADECHANGED,
        PLAYERSHOOT,
        PONG,
        CHANGEPETSKIN,
        TRADEDONE,
        ENEMYSHOOT,
        ACCEPTTRADE,
        CHANGEGUILDRANK,
        PLAYSOUND,
        SQUAREHIT,
        NEWABILITY,
        UPDATE,
        TEXT,
        RECONNECT,
        DEATH,
        USEPORTAL,
        GOTOQUESTROOM,
        ALLYSHOOT,
        RESKIN,
        RESETDAILYQUESTS,
        INVENTORYSWAP,
        CHANGETRADE,
        CREATE,
        QUESTREDEEM,
        CREATEGUILD,
        SETCONDITION,
        LOAD,
        MOVE,
        KEYINFORESPONSE,
        AOE,
        GOTOACK,
        NOTIFICATION,
        CLIENTSTAT,
        HELLO,
        DAMAGE,
        ACTIVEPET,
        INVITEDTOGUILD,
        PETYARDUPDATE,
        PASSWORDPROMPT,
        UPDATEACK,
        QUESTOBJECTID,
        PIC,
        HEROLEFT,
        BUY,
        TRADESTART,
        EVOLVEDPET,
        TRADEREQUESTED,
        AOEACK,
        PLAYERHIT,
        CANCELTRADE,
        MAPINFO,
        KEYINFOREQUEST,
        INVENTORYRESULT,
        QUESTREDEEMRESPONSE,
        CHOOSENAME,
        QUESTFETCHASK,
        ACCOUNTLIST,
        CREATESUCCESS,
        CHECKCREDITS,
        GROUNDDAMAGE,
        GUILDINVITE,
        ESCAPE,
        FILE,
        UNLOCKCUSTOMIZATION,
        NEWCHARACTERINFORMATION,
        UNLOCKNEWSLOT,
        QUEUE,
        QUEUECANCEL,
        EXALTATIONBONUSCHANGED,
        REDEEMEXALTATIONREWARD,
        EXALTATIONREDEEMINFO,
        VAULTCONTENT,
        FORGEREQUEST,
        FORGERESULT,
        FORGEUNLOCKEDBLUEPRINTS,
        SHOOTACKCOUNTER,
        CHANGEALLYSHOOT,
        PLAYERSLIST,
        MODERATORACTION,
        GETPLAYERSLIST,
        CREEPMOVE,
        CUSTOMMAPDELETE,
        CUSTOMMAPDELETERESPONSE,
        CUSTOMMAPLIST,
        CUSTOMMAPLISTRESPONSE,
        CREEPHIT,
        PLAYERCALLOUT,
        REFINERESULT,
        BUYREFINEMENT,
        STARTUSE,
        ENDUSE,
        STACKS,
        BUYITEM,
        BUYITEMRESULT,
        DRAWDEBUGSHAPE,
        DRAWDEBUGARROW,
        DASHRESET,
        FAVORPET,
        SKINRECYCLE,
        SKINRECYCLERESPONSE,
        DAMAGEBOOST,
        CLAIMBPMILESTONE,
        CLAIMBPMILESTONERESULT,
        BOOSTBPMILESTONE,
        BOOSTBPMILESTONERESULT,
        ACCELERATORADDED,
        UNSEASONREQUEST,
        RETITLE,
        SETGRAVESTONE,
        SETABILITY,
        MISSIONPROGRESSUPDATE,
        EMOTE,
        BUYEMOTE,
        SETTRACKEDSEASON,
        CLAIMMISSION,
        CLAIMMISSIONRESULT,
        MULTIPLEMISSIONSPROGRESSUPDATE,
        DAMAGEWITHEFFECT,
        SETDISCOVERABLE,
        REALMSCOREUPDATE,
        CLAIMREWARDSINFOPROMPT,
        CLAIMCHESTREWARDSUBMIT,
        CHESTREWARDRESULT,
        UNLOCKENCHANTMENTSLOT,
        UNLOCKENCHANTMENTSLOTRESULT,
        UNLOCKENCHANTMENT,
        UNLOCKENCHANTMENTRESULT,
        APPLYENCHANTMENT,
        APPLYENCHANTMENTRESULT,
        ENABLECRUCIBLE,
        CRUCIBLERESULT,
        GETDEFINITION,
        RESULTDEFINITION,
        TUTORIALSTATECHANGED,
        UPGRADEENCHANTER,
        UPGRADEENCHANTERRESULT,
        UPGRADEENCHANTMENT,
        UPGRADEENCHANTMENTRESULT,
        REROLLENCHANTMENTS,
        REROLLENCHANTMENTSRESULT,
        RESETENCHANTMENTSREROLLCOUNT,
        RESETENCHANTMENTREROLLCOUNTRESULT,
        PURCHASEPETSHADER,
        PETSHADERPURCHASERESULT,
        DISMANTLEITEMMESSAGE,
        UNKNOWN196,
        UNKNOWN197,
        UNKNOWN198,
        UNKNOWN199,
        CREATEPARTYMESSAGE,
        PARTYACTION,
        PARTYACTIONRESULT,
        PARTYINVITATIONRESPONSE,
        INCOMINGPARTINVITATION,
        INCOMINGPARTYMEMBERINFO,
        PARTYMEMBERADDED,
        PARTYLISTMESSAGE,
        PARTYJOINREQUEST,
        PARTYREQUESTRESPONSE,
        FORRECONNECTMESSAGE,
        UNKNOWN,
        UNDEFINED
    }
}
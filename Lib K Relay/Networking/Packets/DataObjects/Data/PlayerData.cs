using System.Reflection;
using System.Text;
using Lib_K_Relay.Networking.Packets.DataObjects.Stat;
using Lib_K_Relay.Networking.Packets.Server;
using Lib_K_Relay.Utilities;

namespace Lib_K_Relay.Networking.Packets.DataObjects.Data
{
    public class PlayerData // TODO: Add the rest of the stats
    {
        public int AccountFame;

        public string AccountId;

        // future-proofing: technically a possible Player value, but never ends up being one
        public int Attack;
        public int AttackBonus;
        public int[] Backpack = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        public int Breath;
        public int ChallengerStarBg;
        public int CharacterFame;
        public int CharacterFameGoal;
        public Classes Class;
        public int Defense;
        public int DefenseBonus;
        public int Dexterity;
        public int DexterityBonus;
        public int[] Effects = new int[2];
        public float ExaltationDamageMultiplier;
        public int ExaltedAttack;
        public int ExaltedDefense;
        public int ExaltedDexterity;
        public int ExaltedHealth;
        public int ExaltedMana;
        public int ExaltedSpeed;
        public int ExaltedVitality;
        public int ExaltedWisdom;
        public int Forgefire;
        public int FortuneTokens;
        public string GuildName;
        public int GuildRank;
        public bool HasBackpack;
        public bool HasQuickslotUpgrade;
        public bool HasXpBoost;
        public int Health;
        public int HealthBonus;
        public string PetName;
        public int[] Inventory = { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 };
        public int LegendaryRank;
        public int Level = 1;
        public int LootDropBoostTime;
        public int LootTierBoostTime;
        public int Mana;
        public int ManaBonus;
        public int MapHeight;
        public string MapName;
        public int MapWidth;
        public int MaxHealth;
        public int MaxMana;
        public string Name;
        public bool NameChosen;
        public int OwnerObjectId;
        public string OwnerAccountId;
        public string GraveAccountId;
        public Location.Location Pos = new Location.Location();
        public int[] Quickslots = { -1, -1, -1 };
        public int RealmGold;
        public int SinkLevel;
        public int Size;
        public int Speed;
        public int SpeedBonus;
        public int Stars;
        public int SupporterPoints;
        public bool TeleportAllowed;
        public int Unknown23;
        public int Unknown24;
        public int Unknown25;
        public int Texture1;
        public string Texture;
        public int Texture2;
        public int Texture3;
        public int Vitality;
        public int VitalityBonus;
        public int Wisdom;
        public int WisdomBonus;
        public string Xp;
        public int XpBoostTime;
        public int XpGoal;
        public string DungeonMod;
        public int Unknown122;
        public int Unknown123;
        public int Unknown124;
        public int Unknown125;
        public int Unknown126;

        public PlayerData(int ownerObjectId)
        {
            OwnerObjectId = ownerObjectId;
            Name = "";
        }

        public PlayerData(int ownerObjectId, MapInfoPacket mapInfo)
        {
            OwnerObjectId = ownerObjectId;
            Name = "";
            MapName = mapInfo.Name;
            TeleportAllowed = mapInfo.AllowPlayerTeleport;
            MapWidth = mapInfo.Width;
            MapHeight = mapInfo.Height;
        }

        public void Parse(UpdatePacket update)
        {
            foreach (var newObject in update.NewObjs)
                if (newObject.Status.ObjectId == OwnerObjectId)
                {
                    Class = (Classes)newObject.ObjectType;
                    foreach (var data in newObject.Status.Data) Parse(data.Id, data.IntValue, data.StringValue);
                }
        }

        public void Parse(NewTickPacket newTick)
        {
            foreach (var status in newTick.Statuses)
                if (status.ObjectId == OwnerObjectId)
                    foreach (var data in status.Data)
                    {
                        Pos = status.Position;
                        Parse(data.Id, data.IntValue, data.StringValue);
                    }
        }

        public void Parse(int id, int intValue, string stringValue)
        {
            switch (id)
            {
                case (int)StatsType.Stats.HpStat: // 0
                    Health = intValue;
                    break;
                case (int)StatsType.Stats.MaxMpStat: // 3
                    MaxMana = intValue;
                    break;
                case (int)StatsType.Stats.MpStat: // 4
                    Mana = intValue;
                    break;
                case (int)StatsType.Stats.NextLevelExpStat: // 5
                    XpGoal = intValue;
                    break;
                case (int)StatsType.Stats.ExpStat: // 6
                    Xp = stringValue;
                    break;
                case (int)StatsType.Stats.LevelStat: // 7
                    Level = intValue;
                    break;
                case (int)StatsType.Stats.InventoryStat_0: // 8
                case (int)StatsType.Stats.InventoryStat_1: // 9
                case (int)StatsType.Stats.InventoryStat_2: // 10
                case (int)StatsType.Stats.InventoryStat_3: // 11
                case (int)StatsType.Stats.InventoryStat_4: // 12
                case (int)StatsType.Stats.InventoryStat_5: // 13
                case (int)StatsType.Stats.InventoryStat_6: // 14
                case (int)StatsType.Stats.InventoryStat_7: // 15
                case (int)StatsType.Stats.InventoryStat_8: // 16
                case (int)StatsType.Stats.InventoryStat_9: // 17
                case (int)StatsType.Stats.InventoryStat_10: // 18
                case (int)StatsType.Stats.InventoryStat_11: // 19
                    Inventory[id - (int)StatsType.Stats.InventoryStat_0] = intValue;
                    break;
                case (int)StatsType.Stats.AttackStat: // 
                    Attack = intValue;
                    break;
                case (int)StatsType.Stats.DefenseStat: // 
                    Defense = intValue;
                    break;
                case (int)StatsType.Stats.SpeedStat: // 
                    Speed = intValue;
                    break;
                case (int)StatsType.Stats.VitalityStat: // 
                    Vitality = intValue;
                    break;
                case (int)StatsType.Stats.WisdomStat: // 
                    Wisdom = intValue;
                    break;
                case (int)StatsType.Stats.DexterityStat: // 
                    Dexterity = intValue;
                    break;
                case (int)StatsType.Stats.ConditionStat: // 
                    Effects[0] = intValue;
                    break;
                case (int)StatsType.Stats.NumStarsStat: // 
                    Stars = intValue;
                    break;
                case (int)StatsType.Stats.NameStat: // 
                    Name = stringValue;
                    break;
                case (int)StatsType.Stats.CreditsStat: // 
                    RealmGold = intValue;
                    break;
                case (int)StatsType.Stats.AccountIdStat: // 
                    AccountId = stringValue;
                    break;
                case (int)StatsType.Stats.FameStat: // 
                    AccountFame = intValue;
                    break;
                case (int)StatsType.Stats.PetNameStat: // 
                    PetName = stringValue;
                    break;
                case (int)StatsType.Stats.MaxHpBoostStat: // 
                    HealthBonus = intValue;
                    break;
                case (int)StatsType.Stats.MaxMpBoostStat: // 
                    ManaBonus = intValue;
                    break;
                case (int)StatsType.Stats.AttackBoostStat: // 
                    AttackBonus = intValue;
                    break;
                case (int)StatsType.Stats.DefenseBoostStat: // 
                    DefenseBonus = intValue;
                    break;
                case (int)StatsType.Stats.SpeedBoostStat: // 
                    SpeedBonus = intValue;
                    break;
                case (int)StatsType.Stats.VitalityBoostStat: // 
                    VitalityBonus = intValue;
                    break;
                case (int)StatsType.Stats.WisdomBoostStat: // 
                    WisdomBonus = intValue;
                    break;
                case (int)StatsType.Stats.DexterityBoostStat: // 
                    DexterityBonus = intValue;
                    break;
                case (int)StatsType.Stats.NameChosenStat: // 
                    NameChosen = intValue > 0;
                    break;
                case (int)StatsType.Stats.CurrFameStat: // 
                    CharacterFame = intValue;
                    break;
                case (int)StatsType.Stats.NextClassQuestFameStat: // 
                    CharacterFameGoal = intValue;
                    break;
                case (int)StatsType.Stats.LegendaryRankStat: // 
                    LegendaryRank = intValue;
                    break;
                case (int)StatsType.Stats.GuildNameStat: // 
                    GuildName = stringValue;
                    break;
                case (int)StatsType.Stats.GraveAccountId: // 
                    GraveAccountId = stringValue;
                    break;
                case (int)StatsType.Stats.OwnerAccountIdStat: // 
                    OwnerAccountId = stringValue;
                    break;
                case (int)StatsType.Stats.GuildRankStat: // 
                    GuildRank = intValue;
                    break;
                case (int)StatsType.Stats.BreathStat: // 
                    Breath = intValue;
                    break;
                case (int)StatsType.Stats.BackpackStat_0: // 
                case (int)StatsType.Stats.BackpackStat_1: // 
                case (int)StatsType.Stats.BackpackStat_2: // 
                case (int)StatsType.Stats.BackpackStat_3: // 
                case (int)StatsType.Stats.BackpackStat_4: // 
                case (int)StatsType.Stats.BackpackStat_5: // 
                case (int)StatsType.Stats.BackpackStat_6: // 
                case (int)StatsType.Stats.BackpackStat_7:
                case (int)StatsType.Stats.BackpackStat_8:
                case (int)StatsType.Stats.BackpackStat_9:
                case (int)StatsType.Stats.BackpackStat_10:
                case (int)StatsType.Stats.BackpackStat_11:
                case (int)StatsType.Stats.BackpackStat_12:
                case (int)StatsType.Stats.BackpackStat_13:
                case (int)StatsType.Stats.BackpackStat_14:
                case (int)StatsType.Stats.BackpackStat_15:
                    Backpack[id - (int)StatsType.Stats.BackpackStat_0] = intValue;
                    break;
                case (int)StatsType.Stats.BackpackSlotsStat: // 
                    HasBackpack = intValue > 0;
                    break;
                case (int)StatsType.Stats.SinkLevelStat: // 
                    SinkLevel = intValue;
                    break;
                case (int)StatsType.Stats.SizeStat: // 
                    Size = intValue;
                    break;
                case (int)StatsType.Stats.PotionOneType: // 
                case (int)StatsType.Stats.PotionTwoType: // 
                case (int)StatsType.Stats.PotionThreeType: // 
                    Quickslots[id - (int)StatsType.Stats.PotionOneType] = intValue;
                    break;
                case (int)StatsType.Stats.PotionBelt: // 
                    HasQuickslotUpgrade = intValue > 0;
                    break;
                case (int)StatsType.Stats.ExaltedAtk: // 
                    ExaltedAttack = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedDef: // 
                    ExaltedDefense = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedDex: // 
                    ExaltedDexterity = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedHp: // 
                    ExaltedHealth = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedMp: // 
                    ExaltedMana = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedSpd: // 
                    ExaltedSpeed = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedVit: // 
                    ExaltedVitality = intValue;
                    break;
                case (int)StatsType.Stats.ExaltedWis: // 
                    ExaltedWisdom = intValue;
                    break;
                case (int)StatsType.Stats.NewConStat: // 
                    Effects[1] = intValue;
                    break;
                case (int)StatsType.Stats.FortuneTokenStat: // 97
                    FortuneTokens = intValue;
                    break;
                case (int)StatsType.Stats.ExaltationBonusDmg: // 113
                    ExaltationDamageMultiplier = intValue / 1000f;
                    break;
                case (int)StatsType.Stats.SupporterPointsStat: // 98
                    SupporterPoints = intValue;
                    break;
                case (int)StatsType.Stats.XpBoostedStat: // 65
                    HasXpBoost = intValue > 0;
                    break;
                case (int)StatsType.Stats.XpTimerStat: // 66
                    XpBoostTime = intValue * 1000;
                    break;
                case (int)StatsType.Stats.LdTimerStat: // 67
                    LootDropBoostTime = intValue * 1000;
                    break;
                case (int)StatsType.Stats.LtTimerStat: // 68
                    LootTierBoostTime = intValue * 1000;
                    break;
                case (int)StatsType.Stats.CrucibleStat: // 100
                    ChallengerStarBg = intValue;
                    break;
                case (int)StatsType.Stats.ForgeFire: // 120
                    Texture = stringValue;
                    break;
            }
        }

        public bool HasConditionEffect(ConditionEffectIndex effect)
        {
            return (int)effect > 30 ? (Effects[1] & (int)effect) != 0 : (Effects[0] & (int)effect) != 0;
        }

        public override string ToString()
        {
            // Use reflection to get the the non-null fields and arrange them into a table.
            var fields = GetType().GetFields(BindingFlags.Public |
                                             BindingFlags.NonPublic |
                                             BindingFlags.Instance);

            var s = new StringBuilder();
            s.Append(OwnerObjectId + "'s PlayerData Instance");
            foreach (var f in fields)
                if (f.GetValue(this) != null)
                    s.Append("\n\t" + f.Name + " => " + f.GetValue(this));

            return s.ToString();
        }
    }
}
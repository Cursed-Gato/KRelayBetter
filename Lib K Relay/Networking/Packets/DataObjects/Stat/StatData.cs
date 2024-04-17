using System;
using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.DataObjects.Stat
{
    public class StatData : IDataObject {
        public StatsType Id;
        public int IntValue;
        public int StackCount;
        public string StringValue;

        public IDataObject Read(PacketReader r) {
            Id = r.ReadByte();
            if (IsStringData())
                StringValue = r.ReadString();
            else
                IntValue = r.ReadCompressedInt();

            StackCount = r.ReadCompressedInt();

            return this;
        }

        public void Write(PacketWriter w) {
            w.Write(Id);
            if (IsStringData())
                w.Write(StringValue);
            else
                w.WriteCompressedInt(IntValue);

            w.WriteCompressedInt(StackCount);
        }

        public object Clone() {
            return new StatData {
                Id = Id,
                IntValue = IntValue,
                StringValue = StringValue,
                StackCount = StackCount
            };
        }

        public bool IsStringData() {
            return Id.IsUtf();
        }

        public override string ToString() {
            return "{ Id=" + Id + " Value=" + (IsStringData() ? StringValue : IntValue.ToString()) +
                   " StackCount=" + StackCount + " }";
        }
    }

    public partial class StatsType
    {
        private readonly byte _mType;
        
        private StatsType(byte type)
        {
            _mType = type;
        }

        public bool IsUtf()
        {
            return (int)this == (int)Stats.ExpStat
                   || (int)this == (int)Stats.NameStat
                   || (int)this == (int)Stats.AccountIdStat
                   || (int)this == (int)Stats.OwnerAccountIdStat
                   || (int)this == (int)Stats.GuildNameStat
                   || (int)this == (int)Stats.MaterialStat
                   || (int)this == (int)Stats.MaterialCapStat
                   || (int)this == (int)Stats.UniqueDataStr
                   || (int)this == (int)Stats.GraveAccountId
                   || (int)this == (int)Stats.ModifiersStat
                   || (int)this == (int)Stats.DustStat
                   || (int)this == (int)Stats.CrucibleStat
                   || (int)this == (int)Stats.DustAmountStat
                   || (int)this == (int)Stats.PetNameStat;
        }

        public static implicit operator StatsType(int type)
        {
            if (type > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return new StatsType((byte)type);
        }

        public static implicit operator StatsType(byte type)
        {
            return new StatsType(type);
        }

        public static bool operator ==(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return !(type is null) && type._mType == (byte)id;
        }

        public static bool operator ==(StatsType type, byte id)
        {
            return !(type is null) && type._mType == id;
        }

        public static bool operator !=(StatsType type, int id)
        {
            if (id > byte.MaxValue) throw new Exception("Not a valid StatData number.");
            return !(type is null) &&
                   type._mType != (byte)id;
        }

        public static bool operator !=(StatsType type, byte id)
        {
            return !(type is null) && 
                   type._mType != id;
        } 

        public static bool operator ==(StatsType type, StatsType id)
        {
            return !(id is null) && !(type is null) &&
                   type._mType == id._mType;
        }

        public static bool operator !=(StatsType type, StatsType id)
        {
            return !(id is null) && !(type is null) &&
                   type._mType != id._mType;
        }

        public static implicit operator int(StatsType type)
        {
            return type._mType;
        }

        public static implicit operator byte(StatsType type)
        {
            return type._mType;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is StatsType type)) return false;

            return this == type;
        }
        
        public override int GetHashCode()
        {
            return _mType.GetHashCode();
        }

        public override string ToString()
        {
            return _mType.ToString();
        }
    }
}
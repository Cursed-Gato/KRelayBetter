using System.Text;

namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class DailyQuest : IDataObject
    {
        public string Id;
        public string Name;
        public string Description;
        public string Expriation;
        public int Weight;
        public int Category;
        public int[] Requirments;
        public int[] Rewards;
        public bool Completed;
        public bool ItemOfChoice;
        public bool Repeatable;

        public IDataObject Read(PacketReader r)
        {
            Id = r.ReadString();
            Name = r.ReadString();
            Description = r.ReadString();
            Expriation = r.ReadString();
            Weight = r.ReadInt32();
            Category = r.ReadInt32();
            Requirments = new int[r.ReadInt16()];
            for (int i = 0; i < Requirments.Length; i++)
            {
                Requirments[i] = r.ReadInt32();
            }
            Rewards = new int[r.ReadInt16()];
            for (int i = 0; i < Rewards.Length; i++)
            {
                Rewards[i] = r.ReadInt32();
            }
            Completed = r.ReadBoolean();
            ItemOfChoice = r.ReadBoolean();
            Repeatable = r.ReadBoolean();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(Id);
            w.Write(Name);
            w.Write(Description);
            w.Write(Expriation);
            w.Write(Weight);
            w.Write(Category);
            w.Write((short)Requirments.Length);
            foreach (var req in Requirments)
            {
                w.Write(req);
            }
            w.Write((short)Rewards.Length);
            foreach (var reward in Rewards)
            {
                w.Write(reward);
            }
            w.Write(Completed);
            w.Write(ItemOfChoice);
            w.Write(Repeatable);
        }

        public object Clone()
        {
            return new DailyQuest
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Weight = Weight,
                Category = Category,
                Requirments = (int[])Requirments.Clone(),
                Rewards = (int[])Rewards.Clone(),
                Completed = Completed,
                ItemOfChoice = ItemOfChoice,
                Repeatable = Repeatable
            };
        }

        public override string ToString()
        {
            return "{ Id=" + Id + ", Name=" + Name + ", Description=" + Description +
                   ", Weight=" + Weight + ", Category=" + Category +
                   ", Requirements=[" + string.Join(", ", Requirments) +
                   "], Rewards=[" + string.Join(", ", Rewards) +
                   "], Completed=" + Completed + ", ItemOfChoice=" + ItemOfChoice +
                   ", Repeatable=" + Repeatable + " }";
        }
    }
}
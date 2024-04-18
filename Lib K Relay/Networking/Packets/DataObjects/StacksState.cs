namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class StacksState : IDataObject
    {
        public byte Hp;
        public byte Mp;
        public byte Attack;
        public byte Defense;
        public byte Speed;
        public byte Vitality;
        public byte Wisdom;
        public byte Dexterity;

        public IDataObject Read(PacketReader r)
        {
            Hp = r.ReadByte();
            Mp = r.ReadByte();
            Attack = r.ReadByte();
            Defense = r.ReadByte();
            Speed = r.ReadByte();
            Vitality = r.ReadByte();
            Wisdom = r.ReadByte();
            Dexterity = r.ReadByte();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(Hp);
            w.Write(Mp);
            w.Write(Attack);
            w.Write(Defense);
            w.Write(Speed);
            w.Write(Vitality);
            w.Write(Wisdom);
            w.Write(Dexterity);
        }

        public object Clone()
        {
            return new StacksState
            {
                Hp = Hp,
                Mp = Mp,
                Attack = Attack,
                Defense = Defense,
                Speed = Speed,
                Vitality = Vitality,
                Wisdom = Wisdom,
                Dexterity = Dexterity
            };
        }

        public override string ToString()
        {
            return "{ Hp=" + Hp + ", Mp=" + Mp + ", Attack=" + Attack + ", Defense=" +
                   Defense + ", Speed=" + Speed + ", Vitality=" + Vitality +
                   ", Wisdom=" + Wisdom + ", Dexterity=" + Dexterity + " }";
        }
    }
}
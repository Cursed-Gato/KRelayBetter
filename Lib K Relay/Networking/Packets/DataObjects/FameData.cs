namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class FameData : IDataObject
    {
        public string Id;
        public int Repitions;
        public int Fame;

        public IDataObject Read(PacketReader r)
        {
            Id = r.ReadString();
            Repitions = r.ReadCompressedInt();
            Fame = r.ReadCompressedInt() / Repitions;

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(Id);
            w.Write(Repitions);
            w.Write(Fame * Repitions);
        }

        public object Clone()
        {
            return new FameData
            {
                Id = Id,
                Repitions = Repitions,
                Fame = Fame
            };
        }

        public override string ToString()
        {
            return "{ Id=" + Id + ", Repitions=" + Repitions + ", Fame=" + Fame +  "}";
        }
    }
}

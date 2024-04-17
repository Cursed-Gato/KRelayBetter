using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.DataObjects.Stat
{
    public class Status : IDataObject
    {
        public StatData[] Data;
        public int ObjectId;
        public Location.Location Position = new Location.Location();

        public IDataObject Read(PacketReader r)
        {
            ObjectId = r.ReadCompressedInt();
            Position.Read(r);
            Data = new StatData[r.ReadCompressedInt()];

            for (var i = 0; i < Data.Length; i++)
            {
                var statData = new StatData();
                statData.Read(r);
                Data[i] = statData;
            }

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.WriteCompressedInt(ObjectId);
            Position.Write(w);
            w.WriteCompressedInt(Data.Length);

            foreach (var statData in Data)
                statData.Write(w);
        }

        public object Clone()
        {
            return new Status
            {
                Data = (StatData[])Data.Clone(),
                ObjectId = ObjectId,
                Position = (Location.Location)Position.Clone()
            };
        }
    }
}
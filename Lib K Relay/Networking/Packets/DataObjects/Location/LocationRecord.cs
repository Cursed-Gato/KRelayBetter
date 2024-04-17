using Lib_K_Relay.Networking.Packets.DataObjects.Data;

namespace Lib_K_Relay.Networking.Packets.DataObjects.Location
{
    public class LocationRecord : Location
    {
        public int Time;

        public override IDataObject Read(PacketReader r)
        {
            Time = r.ReadInt32();
            base.Read(r);

            return this;
        }

        public override void Write(PacketWriter w)
        {
            w.Write(Time);
            base.Write(w);
        }

        public override object Clone()
        {
            return new LocationRecord
            {
                Time = Time,
                X = X,
                Y = Y
            };
        }

        public override string ToString()
        {
            return "{ Time=" + Time + ", X=" + X + ", Y=" + Y + " }";
        }
    }
}
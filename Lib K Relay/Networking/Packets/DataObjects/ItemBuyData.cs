using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class ItemBuyData : IDataObject
    {
        public const byte DyeCategory = 0;
        public const byte ShaderCategory = 1;

        public byte Category;
        public byte Currency;
        public int ObjectId;

        public IDataObject Read(PacketReader r)
        {
            Category = r.ReadByte();
            ObjectId = r.ReadInt32();
            Currency = r.ReadByte();

            return this;
        }

        public void Write(PacketWriter w)
        {
            w.Write(Category);
            w.Write(ObjectId);
            w.Write(Currency);
        }

        public object Clone()
        {
            return new ItemBuyData
            {
                Category = Category,
                ObjectId = ObjectId,
                Currency = Currency
            };
        }

        public override string ToString()
        {
            return "{ Category=" + Category + ", ObjectId=" + ObjectId + ", Currency=" + Currency + " }";
        }
    }
}


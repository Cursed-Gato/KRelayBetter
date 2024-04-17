using Lib_K_Relay.Networking.Packets.DataObjects.Data;
using Lib_K_Relay.Networking.Packets.DataObjects.Location;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class UpdatePacket : Packet
    {
        public Location Position;
        public byte LevelType;
        public int[] Drops;
        public Entity[] NewObjs;
        public Tile[] Tiles;
        public override PacketType Type => PacketType.UPDATE;

        public override void Read(PacketReader r)
        {
            Position = (Location) new Location().Read(r);
            LevelType = r.ReadByte();
            Tiles = new Tile[r.ReadCompressedInt()];
            for (var i = 0; i < Tiles.Length; i++)
                Tiles[i] = (Tile) new Tile().Read(r);

            NewObjs = new Entity[r.ReadCompressedInt()];
            for (var j = 0; j < NewObjs.Length; j++)
                NewObjs[j] = (Entity) new Entity().Read(r);

            Drops = new int[r.ReadCompressedInt()];
            for (var k = 0; k < Drops.Length; k++)
                Drops[k] = r.ReadCompressedInt();
        }

        public override void Write(PacketWriter w)
        {
            Position.Write(w);
            w.Write(LevelType);
            w.WriteCompressedInt(Tiles.Length);
            foreach (var t in Tiles)
                t.Write(w);

            w.WriteCompressedInt(NewObjs.Length);
            foreach (var e in NewObjs)
                e.Write(w);

            w.WriteCompressedInt(Drops.Length);
            foreach (var i in Drops)
                w.WriteCompressedInt(i);
        }
    }
}
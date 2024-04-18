using System.ComponentModel;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class GetDefinitionPacket : Packet
    {
        public int[] Types;

        public override PacketType Type => PacketType.GETDEFINITION;

        public override void Read(PacketReader r)
        {
            Types = new int[r.ReadInt16()];
            for(int i = 0; i < Types.Length; i++) 
            {
                Types[i] = r.ReadInt32();
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)Types.Length);
            for(int i = 0; i < Types.Length; i++)
            {
                w.Write(Types[i]);
            }
        }
    }
}
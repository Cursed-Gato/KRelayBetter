namespace Lib_K_Relay.Networking.Packets.Server
{
    public class ResultDefinitionPacket : Packet
    {
        public int[] DefintitionsIds;
        public string[] DefinitionStrings;


        public override PacketType Type => PacketType.RESULTDEFINITION;

        public override void Read(PacketReader r)
        {
            DefintitionsIds = new int[r.ReadInt16()];
            for (int i = 0; i < DefintitionsIds.Length; i++)
            {
                DefintitionsIds[i] = r.ReadInt32();
            }

            DefinitionStrings = new string[r.ReadInt16()];
            for (int i = 0; i < DefinitionStrings.Length; i++)
            {
                DefinitionStrings[i] = r.ReadString();
            }
        }

        public override void Write(PacketWriter w)
        {
            w.Write((short)DefintitionsIds.Length);
            for(int i = 0;i < DefintitionsIds.Length; i++)
            {
                w.Write(DefintitionsIds[i]);
            }

            w.Write((short)DefinitionStrings.Length);
            for (int i = 0; i < DefinitionStrings.Length; i++)
            {
                w.Write(DefinitionStrings[i]);
            }
        }
    }
}
namespace Lib_K_Relay.Networking.Packets.Server
{
    public class AccountListPacket : Packet
    {
        public string[] AccountIds;
        public int AccountListId;
        public int LockAction;

        public override PacketType Type => PacketType.ACCOUNTLIST;

        public override void Read(PacketReader r)
        {
            AccountListId = r.ReadInt32();
            AccountIds = new string[r.ReadUInt16()];
            for (var i = 0; i < AccountIds.Length; i++)
                AccountIds[i] = r.ReadString();

            LockAction = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(AccountListId);
            w.Write((ushort)AccountIds.Length);
            foreach (var i in AccountIds) w.Write(i);

            w.Write(LockAction);
        }
    }
}
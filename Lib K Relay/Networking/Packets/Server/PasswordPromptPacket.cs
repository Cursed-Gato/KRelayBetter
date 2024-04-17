namespace Lib_K_Relay.Networking.Packets.Server
{
    public class PasswordPromptPacket : Packet
    {
        public const int SignIn = 2;
        public const int SendEmailAndSignIn = 3;
        public const int Register = 4;

        public int CleanPasswordStatus;

        public override PacketType Type => PacketType.PASSWORDPROMPT;

        public override void Read(PacketReader r)
        {
            CleanPasswordStatus = r.ReadInt32();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CleanPasswordStatus);
        }
    }
}
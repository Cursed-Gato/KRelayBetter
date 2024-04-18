using Lib_K_Relay.Networking.Packets.DataObjects;

namespace Lib_K_Relay.Networking.Packets.Server
{
    public class StacksPacket : Packet
    {
        public int CharId;
        public StacksState State;

        public override PacketType Type => PacketType.STACKS;

        public override void Read(PacketReader r)
        {
            CharId = r.ReadCompressedInt();
            State = (StacksState)new StacksState().Read(r);
        }

        public override void Write(PacketWriter w)
        {
            w.Write(CharId);
            State.Write(w);
        }
    }
}
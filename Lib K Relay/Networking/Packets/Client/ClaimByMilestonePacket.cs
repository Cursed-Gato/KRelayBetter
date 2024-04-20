using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib_K_Relay.Networking.Packets.Client
{
    public class ClaimByMilestonePacket : Packet
    {
        public sbyte MilestoneId;

        public override PacketType Type => PacketType.CLAIMBPMILESTONE;

        public override void Read(PacketReader r)
        {
            MilestoneId = r.ReadSByte();
        }

        public override void Write(PacketWriter w)
        {
            w.Write(MilestoneId);
        }
    }
}

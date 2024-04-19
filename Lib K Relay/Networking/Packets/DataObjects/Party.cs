
namespace Lib_K_Relay.Networking.Packets.DataObjects
{
    public class Party : IDataObject
	{
		public string Descripition;
		public uint PartyId;
		public ushort Powerlevel;
		public byte CurrentPlayers;
		public byte MaxPlayers;
		public byte PartyType;
		public byte Privacy; //1 = public 2 = private
		public byte RequiredMaxedStats;
		public byte Server;

		public IDataObject Read(PacketReader r)
		{
            Descripition = r.ReadString();
            PartyId = r.ReadUInt32();
            Powerlevel = r.ReadUInt16();
            CurrentPlayers = r.ReadByte();
            MaxPlayers = r.ReadByte();
            PartyType = r.ReadByte();
            Privacy = r.ReadByte();
            RequiredMaxedStats = r.ReadByte();
			Server = r.ReadByte();	

            return this;
		}

		public void Write(PacketWriter w)
		{
			w.Write(Descripition);
			w.Write(PartyId);
			w.Write(Powerlevel);
			w.Write(CurrentPlayers);
			w.Write(MaxPlayers);
			w.Write(PartyType);
			w.Write(Privacy);
			w.Write(RequiredMaxedStats);
            w.Write(Server);
        }


		public object Clone()
		{
			return new Party
			{
                Descripition = Descripition,
                PartyId = PartyId,
                Powerlevel = Powerlevel,
                CurrentPlayers = CurrentPlayers,
                MaxPlayers = MaxPlayers,
                PartyType = PartyType,
                Privacy = Privacy,
                RequiredMaxedStats = RequiredMaxedStats,
                Server = Server
            };
		}

		public override string ToString()
		{
			return "{ Descripition=" + Descripition + ", PartyId=" + PartyId + ", Powerlevel=" + Powerlevel +
                   ", CurrentPlayers=" + CurrentPlayers + ", MaxPlayers=" + MaxPlayers + ", PartyType=" + PartyType +
                   ", Privacy=" + Privacy + ", RequiredMaxedStats=" + RequiredMaxedStats + ", Server=" + Server + " }";
		}
	}
}

#region References

using Server;
using Server.Accounting;

#endregion

namespace Server.Nelderim
{
	class FactionInit
	{
		public static string ModuleName = "Faction";

		public static void Initialize()
		{
			EventSink.WorldSave += Save;
			Faction.Load(ModuleName);
		}
		public static void Save(WorldSaveEventArgs args)
		{
			Faction.Save(args, ModuleName);
		}

		public static void Configure()
		{
			RegisterFaction(new None(0));
			RegisterFaction(new East(1));
			RegisterFaction(new West(2));
			RegisterFaction(new KompaniaHandlowa(3));
			RegisterFaction(new VoxPopuli(4));
		}

		public static void RegisterFaction(Faction faction)
		{
			Faction.Factions[faction.Index] = faction;
			Faction.AllFactions.Add(faction);
		}
	}
}

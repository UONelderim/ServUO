using System.Collections.Generic;
using System.Linq;
using Nelderim;

namespace Server.Nelderim
{
	[Parsable]
	public abstract class Faction : NExtension<FactionInfo>
	{
		public static Faction[] Factions = new Faction[0x100];
		
		public static List<Faction> AllFactions = new List<Faction>();

		public static Faction Default => Factions[0];

		public static Faction None => Factions[0];

		public static Faction East => Factions[1];

		public static Faction West => Factions[2];

		public static Faction KompaniaHandlowa => Factions[3];

		public static Faction VoxPopuli => Factions[4];
		
		protected Faction(int index)
		{
			Index = index;
		}
		
		public int Index { get; }
		
		public abstract string Name { get; }
		
		public abstract Faction[] Enemies { get; }

		public bool IsEnemy(Mobile target)
		{
			return Enemies.Contains(target.Faction);
		}

		public static Faction Parse(string name)
		{
			return AllFactions.Find(f => f.GetType().Name == name) ?? None;
		}
	}
}

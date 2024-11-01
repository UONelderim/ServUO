using System;
using System.Collections.Generic;
using System.Linq;

namespace Server.Nelderim
{
	[Parsable]
	public abstract class Faction(int index) 
	{
		public static Faction[] Factions = new Faction[0x100];
		
		public static List<Faction> AllFactions = new List<Faction>();

		public static Faction Default => Factions[0];

		public static Faction None => Factions[0];

		public static Faction East => Factions[1];

		public static Faction West => Factions[2];

		public static Faction KompaniaHandlowa => Factions[3];

		public static Faction VoxPopuli => Factions[4];

		public int Index { get; } = index;

		public abstract string Name { get; }
		
		public abstract Faction[] Enemies { get; }

		public virtual Race[] Races => Array.Empty<Race>();

		public bool IsFactionRace(Race race) => Races.Contains(race);

		public bool IsEnemy(Mobile target)
		{
			return Enemies.Contains(target.Faction);
		}

		public static Faction Parse(string name)
		{
			return AllFactions.Find(f => 
				f.GetType().Name.Equals(name, StringComparison.InvariantCultureIgnoreCase) || 
				f.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)
				) ?? None;
		}
	}
}

using System.Linq;
using Server.Mobiles;

namespace Server.Commands
{
	public class FixControlSlots
	{
		
		public static void Initialize()
		{
			CommandSystem.Register("FixControlSlots", AccessLevel.Administrator, OnFixControlSlots);
		}

		private static void OnFixControlSlots(CommandEventArgs e)
		{
			var creatures = World.Mobiles.Values.OfType<BaseCreature>();
			foreach (var creature in creatures.ToArray())
			{
				creature.CalculateSlots(creature.ControlSlots);

			}
			e.Mobile.SendMessage("Fixed all");
		}
	}
}

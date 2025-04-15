using System.Linq;
using Server.Mobiles;

namespace Server.Commands
{
	public class FixCreatures
	{
		public static void Initialize()
		{
			CommandSystem.Register("FixCreatures", AccessLevel.Administrator, OnCommand);
		}

		private static void OnCommand(CommandEventArgs e)
		{
			var creatures = World.Mobiles.Values.OfType<BaseCreature>();
			foreach (var creature in creatures.ToArray())
			{
				creature.CalculateSlots(creature.ControlSlots);
				if (creature.ControlSlots > creature.ControlSlotsMin)
				{
					creature.AdjustTameRequirements();
				}
			}
			e.Mobile.SendMessage("Fixed all");
		}
	}
}

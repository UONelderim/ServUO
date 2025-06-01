using System;
using Server;
using Server.Commands;
using Server.Targeting;
using Server.Gumps;
using Server.Items;
using Server.Mobiles;

namespace Server.Scripts.Commands
{
	public static class VoodooCommands
	{
		public static void Initialize()
		{
			// re-register the [gusla command at player level
			CommandSystem.Register(
				"gusla",
				AccessLevel.Player,
				new CommandEventHandler(OnGuslaCommand)
			);
		}

		[Usage("[gusla")]
		[Description("Otwiera gump guślarstwa dla wskazanej laleczki voodoo.")]
		private static void OnGuslaCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;
			from.SendMessage("Wskaż laleczkę guślarza, dla której chcesz otworzyć gump.");
			from.Target = new GuslaDollTarget();
		}

		private class GuslaDollTarget : Target
		{
			public GuslaDollTarget() : base(12, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is VoodooDoll doll && !doll.Deleted)
				{
					// must be animated at least once
					if (doll.Animated < 1)
					{
						from.SendMessage("Ta laleczka nie jest ożywiona - potrzebujesz mikstury animacji.");
						return;
					}

					// if no active link yet, we still open the gump so they can cast “Link”
					from.SendGump(new VoodooSpellGump(doll, from));
				}
				else
				{
					from.SendMessage("To nie jest laleczka guślarza.");
				}
			}
		}
	}
}

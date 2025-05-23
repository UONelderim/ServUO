using System;
using Server;
using Server.Commands;
using Server.Gumps;
using Server.Items;
using Server.Targeting;

namespace Server.Scripts.Commands
{
	public static class VoodooCommands
	{
		public static void Initialize()
		{
			// Rejestrujemy komendę [gusla] dla wszystkich graczy
			CommandSystem.Register(
				"gusla",
				AccessLevel.Player,
				new CommandEventHandler(OnGuslaCommand)
			);
		}

		[Usage("gusla")]
		[Description("Pozwala wskazać lalkę guślarza i otworzyć dla niej gump z zaklęciami.")]
		private static void OnGuslaCommand(CommandEventArgs e)
		{
			Mobile from = e.Mobile;

			// Nowa walidacja umiejętności TasteID
			if (from.Skills.TasteID.Value < 50.0)
			{
				from.SendMessage("Potrzebujesz co najmniej 50.0 w umiejętności Guślarstwa, aby użyć tej komendy.");
				return;
			}

			from.SendMessage("Wskaż lalkę guślarza, dla której chcesz otworzyć gump.");
			from.Target = new GuslaTarget();
		}

		private class GuslaTarget : Target
		{
			public GuslaTarget() : base(1, false, TargetFlags.None)
			{
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is VoodooDoll doll)
				{
					// Otwieramy gump
					from.SendGump(new VoodooSpellGump(from, doll));
				}
				else
				{
					from.SendMessage("To nie jest lalka voodoo.");
				}
			}
		}
	}
}

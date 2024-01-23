#region References

using System;
using System.Collections;
using Server;
using Server.Accounting;
using Server.Commands;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;

#endregion

namespace Nelderim
{
	class Commands
	{
		public static void Initialize()
		{
			CommandSystem.Register("kartapostaci", AccessLevel.Player, KartaPostaci);
			CommandSystem.Register("kp", AccessLevel.Player, KartaPostaci);
			CommandSystem.Register("qs", AccessLevel.GameMaster, Qs_OnCommand);
		}

		[Usage("KartaPostaci")]
		[Description("Otwiera karte postaci gracza")]
		private static void KartaPostaci(CommandEventArgs e)
		{
			if (e.Mobile.AccessLevel == AccessLevel.Player)
				e.Mobile.SendGump(new CharacterSheetGump(e.Mobile, e.Mobile, CSPages.General, false));
			else
				e.Mobile.BeginTarget(18, false, TargetFlags.None, CharacterSheetOfOtherPlayer_OnTarget);
		}

		private static void CharacterSheetOfOtherPlayer_OnTarget(Mobile from, object obj)
		{
			if (obj is PlayerMobile m)
				from.SendGump(new CharacterSheetGump(from, m, CSPages.General, true));
		}

		[Usage("Qs")]
		[Description("Przenosi do losowego gracza, ktory chce uczestniczyc w eventach.")]
		private static void Qs_OnCommand(CommandEventArgs args)
		{
			ArrayList players = new ArrayList();
			Mobile from = args.Mobile;

			try
			{
				foreach (NetState ns in NetState.Instances)
				{
					Mobile m = ns.Mobile;

					if (m != null && from != m && m.AccessLevel == AccessLevel.Player && !from.InRange(m.Location, 5) &&
					    m.Map != Map.Internal && CharacterSheet.Get(m).AttendenceInEvents)
						players.Add(m);
				}

				if (players.Count == 0)
					args.Mobile.SendLocalizedMessage(505707); // "There is noone to visit."
				else
				{
					Mobile m = (Mobile)players[Utility.Random(players.Count)];

					from.MoveToWorld(m.Location, m.Map);

					from.SendLocalizedMessage(505708, m.Name); // Imie: ~1_NAME~
					from.SendLocalizedMessage(505711, ((Account)m.Account).Username); // Konto: ~1_ACCOUNT~
					from.SendLocalizedMessage(505709, m.Race.GetName(Cases.Mianownik)); // Rasa: ~1_RACE~

					string log = args.Mobile.AccessLevel + " " + CommandLogging.Format(args.Mobile);
					log += " randomly moved to player " + CommandLogging.Format(m) + " [Qs]";

					CommandLogging.WriteLine(args.Mobile, log);
				}
			}
			catch (Exception exc)
			{
				from.SendLocalizedMessage(505054); // Wystapil niespodziewany blad polecenia! Zglos go Ekipie.
				Console.WriteLine(exc.ToString());
			}
		}
	}
}

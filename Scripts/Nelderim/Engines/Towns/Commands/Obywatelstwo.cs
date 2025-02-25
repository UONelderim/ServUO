using System;
using Nelderim.Towns;
using Server.Gumps;

namespace Server.Commands
{
	public class Obywatelstwo
	{
		public static void Initialize()
		{
			CommandSystem.Register("Obywatelstwo", AccessLevel.Player, Obywatelstwo_handler);
			CommandSystem.Register("ObywatelstwoPorzuc", AccessLevel.Player, ObywatelstwoPorzuc_handler);
			CommandSystem.Register("ObywatelstwoHourlyCheck", AccessLevel.Administrator,
				ObywatelstwoHourlyCheck_handler);
			CommandSystem.Register("ObywatelstwoDailyCheck", AccessLevel.Administrator, ObywatelstwoDailyCheck_handler);
			CommandSystem.Register("OstatnieWydarzenia", AccessLevel.Player, OstatnieWydarzenia_handler);
		}

		[Usage("Obywatelstwo")]
		[Description("Pozwala sprawdzic nazwe miasta, w ktorym posiada sie obywatelstwo.")]
		private static void Obywatelstwo_handler(CommandEventArgs args)
		{
			Mobile from = args.Mobile;
			if (TownDatabase.IsCitizenOfAnyTown(from))
			{
				Towns fromIsCtizenWhere = TownDatabase.IsCitizenOfWhichTown(from);
				args.Mobile.SendMessage(String.Format("Jestes obywatelem miasta {0}.", fromIsCtizenWhere.ToString()));
			}
			else
			{
				from.SendLocalizedMessage(1063806); // Nie jestes obywatelem zadnego miasta
			}
		}

		[Usage("ObywatelstwoPorzuc")]
		[Description("Pozwala zrzec sie obecnego obywatelstwa w miescie.")]
		private static void ObywatelstwoPorzuc_handler(CommandEventArgs args)
		{
			Mobile from = args.Mobile;
			if (TownDatabase.IsCitizenOfAnyTown(from))
			{
				Towns fromIsCtizenWhere = TownDatabase.IsCitizenOfWhichTown(from);
				from.SendGump(new TownPrompt(fromIsCtizenWhere, from, 1));
			}
			else
			{
				from.SendLocalizedMessage(1063806); // Nie jestes obywatelem zadnego miasta
			}
		}

		[Usage("ObywatelstwoHourlyCheck")]
		[Description("Wywoluje sprawdzenie stanu miast.")]
		private static void ObywatelstwoHourlyCheck_handler(CommandEventArgs args)
		{
			TownAnnouncer.Check();
		}

		[Usage("ObywatelstwoDailyCheck")]
		[Description("Wywoluje sprawdzenie stanu miast.")]
		private static void ObywatelstwoDailyCheck_handler(CommandEventArgs args)
		{
			TownAnnouncer.Check(true);
		}

		[Usage("OstatnieWydarzenia")]
		[Description("Pokazuje ostatnie wydarzenia z miasta, w ktorym gracz jest obywatelem.")]
		private static void OstatnieWydarzenia_handler(CommandEventArgs args)
		{
			Mobile from = args.Mobile;
			if (TownDatabase.IsCitizenOfAnyTown(from))
			{
				from.SendGump(new TownLogGump(TownDatabase.IsCitizenOfWhichTown(from), from));
			}
			else
			{
				from.SendLocalizedMessage(1063806); // Nie jestes obywatelem zadnego miasta
			}
		}
	}
}

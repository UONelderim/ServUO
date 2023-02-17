#region References

using System;
using Server.Commands;
using Server.Nelderim;
using Server.Targeting;

#endregion

namespace Server
{
	public class RaceGenerator
	{
		public static void Initialize()
		{
			CommandSystem.Register("Rasa", AccessLevel.GameMaster, Appearance_OnCommand);
		}

		[Usage("Rasa {<Rasa: none - 0 | Tamael - 1 | Jarling - 2 | Krasnolud - 3 | Elf - 4 | Drow - 5 | Naur - 6>}")]
		[Description(
			"Zmienia wyglad i rase NPCa na losowy zgodny z jego rasa. Jesli z parametrem, to ustawia rase i zmienia wyglad.")]
		private static void Appearance_OnCommand(CommandEventArgs e)
		{
			if (e.Length == 0)
			{
				e.Mobile.SendMessage(
					"Poprawne uzycie: <[Rasa none - 0 | Tamael - 1 | Jarling - 2 | Krasnolud - 3 | Elf - 4 | Drow - 5 | Naur - 6>");
			}
			else
			{
				int par = e.GetInt32(0);

				switch (par)
				{
					case 0:
						e.Mobile.Target = new AppearanceTarget(Race.DefaultRace);
						break;
					case 1:
						e.Mobile.Target = new AppearanceTarget(Race.NTamael);
						break;
					case 2:
						e.Mobile.Target = new AppearanceTarget(Race.NJarling);
						break;
					case 3:
						e.Mobile.Target = new AppearanceTarget(Race.NKrasnolud);
						break;

					case 4:
						e.Mobile.Target = new AppearanceTarget(Race.NElf);
						break;
					case 5:
						e.Mobile.Target = new AppearanceTarget(Race.NDrow);
						break;
					case 6:
						e.Mobile.Target = new AppearanceTarget(Race.NNaur);
						break;
					default:
						e.Mobile.SendMessage("Niepoprawny parametr");
						break;
				}
			}
		}

		private class AppearanceTarget : Target
		{
			private readonly Race m_Race;

			public AppearanceTarget(Race race) : base(-1, false, TargetFlags.None)
			{
				m_Race = race;
			}

			protected override void OnTarget(Mobile from, object targeted)
			{
				if (targeted is Mobile)
				{
					Mobile targ = (Mobile)targeted;

					if (targ.BodyValue == 400 || targ.BodyValue == 401)
					{
						targ.Race = m_Race;
						m_Race.MakeRandomAppearance(targ);

						from.SendMessage("Ustawiono rase: {0}", m_Race);
					}
					else
						from.SendMessage("Wyglada, ze cel nie jest humanoidem!");
				}
			}
		}

		public static void Init(Mobile m)
		{
			try
			{
				if (!m.Deleted)
				{
					m.Female = RegionsEngine.GetFemaleChance(m.Region.Name) > Utility.RandomDouble();

					if(m.Race == Race.DefaultRace)
						m.Race = RegionsEngine.GetRace(m.Region.Name);
					m.Race.MakeRandomAppearance(m);

					if(String.IsNullOrEmpty(m.Name))
						m.Name = NameList.RandomName(m.Race.Name.ToLower() + "_" + (m.Female ? "female" : "male"));
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("RaceGenerator.Init error: " + e);
			}
		}
	}
}

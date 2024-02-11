#region References

using System;
using Nelderim.Races;
using Server.Commands;
using Server.Mobiles;
using Server.Nelderim;
using Server.Targeting;

#endregion

namespace Server
{
	public class RaceGenerator
	{
		public static void Initialize()
		{
			CommandSystem.Register("Rasa", AccessLevel.GameMaster, OnCommand);
		}

		[Usage("Rasa")]
		[Description(
			"Zmienia wyglad i rase NPCa na losowy zgodny z jego rasa. Jesli z parametrem, to ustawia rase i zmienia wyglad.")]
		private static void OnCommand(CommandEventArgs e)
		{
			if (e.Length == 0)
			{
				e.Mobile.SendMessage("Podaj nazwe rasy");
			}
			else
			{
				var name = e.GetString(0);
				var index = NRace.AllRaces.IndexOf(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
				if (index == -1)
				{
					e.Mobile.SendMessage("Niepoprawna nazwa rasy");
				}
				else
				{
					var race = NRace.AllRaces[index];
					e.Mobile.BeginTarget(16,
						false,
						TargetFlags.None,
						((from, targeted) =>
						{
							if (targeted is Mobile m)
							{
								if (m.BodyValue == 400 || m.BodyValue == 401)
								{
									m.Race = race;
									race.MakeRandomAppearance(m);
									if (m is not PlayerMobile)
									{
										race.AssignDefaultLanguages(m);
									}

									from.SendMessage("Ustawiono rase: {0}", race);
								}
								else
									from.SendMessage("Wyglada, ze cel nie jest humanoidem!");
							}
						}));
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
					m.Race.AssignDefaultLanguages(m);

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

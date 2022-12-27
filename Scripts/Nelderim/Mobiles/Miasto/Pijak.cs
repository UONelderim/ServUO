#region References

using System.Collections.Generic;

#endregion

namespace Server.Mobiles
{
	public class Pijak : NBaseTalkingNPC
	{
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{ Race.DefaultRace, new List<Action>() },
			{
				Race.NTamael, new List<Action>
				{
					m => m.Say("Wina, wina, wiiiina dajcie! A jak umrę pochowajcie!"),
					m => m.Say("Wódko ma, wódko ma, wódko ma! Któż bez ciebie sobie w życiu rade da!"),
					m => m.Say("I jeszcze jeden... I Jeszcze raz..."),
					m =>
					{
						m.Say("Świat się zmienia... Słońce zachodzi, a wódka się kończy...");
						m.Emote("*Hic*");
					},
					m => m.Say("Mamusia mówiła... Nie pij z krasnalami"),
					m =>
					{
						m.Say("Dawno nie było tak słabego piwa...");
						m.Emote("*Spogląda na butlę w dłoni*");
						m.Say("Chociaż z drugiej strony...");
						m.Emote("*Przechyla butlę*");
					},
					m => m.Emote("*Czka*"),
					m => m.Say("Dobrodzieju... Nie będę kłamać, zbieram na wino, poratujcie w potrzebie"),
					m =>
					{
						m.Say("Czy mam problem z alkoholem?");
						m.Emote("*Zerka na pustą flaszeczkę*");
						m.Say("Owszem... Skończył się");
					},
					m => m.Say("No dobra... Chluśniem bo uśniem"),
					m =>
					{
						m.Say("Pierdykniem bo odwykniem");
						m.Emote("*Pogrąża się w upojeniu*");
					},
					m => m.Say("Wyglądasz na światowca! Prawda to, że kurwy z Garlan goliły się tam na dole?"),
					m => m.Emote("*Smarka w brudną chustę*"),
					m =>
					{
						m.Say("Kiedyś to było...");
						m.Emote("*Wzdycha ciężko*");
					},
					m => m.Emote("*Poprawia łachmany*"),
					m => m.Say("Co mi się tak pić chce? Przecież wczoraj tyle sie piło..."),
					m =>
					{
						m.Emote("*Podnosi coś z ziemi*");
						m.Say("Ah.. jednak nie");
					},
					m =>
					{
						m.Say("Czuję się, jakby mnie ktoś zjadł, a potem wysrał...");
						m.Emote("*Rozgląda się powoli wzdychając*");
					},
					m =>
					{
						m.Say("Jak nie chlapne sobie winka, to nie pozna mnie rodzinka");
						m.Emote("*Śmieje sie głośno*");
					},
				}
			},
			{
				Race.NJarling, new List<Action>
				{
					m => m.Say("Wódka jak wódka, smakuje jednakowo... czyli wspaniale"),
					m =>
					{
						m.Say("Czym sie strułeś tym sie lecz jak to mamusia mówiła");
						m.Emote("*Otwiera flaszeczkę wódki*");
					},
					m =>
					{
						m.Say("Mianowicie, czas na picie");
						m.Emote("*Brechta*");
					},
					m =>
					{
						m.Say("Jak sie nie pije to wątroba gnije!");
						m.Emote("*Rechocze*");
					},
					m =>
					{
						m.Emote("*Zatacza sie*");
						m.Say("Buja jak na morzu...");
						m.Emote("*Hic*");
					},
					m =>
					{
						m.Say("Picie to jest życie! A jak!");
						m.Emote("*Szczerzy sie*");
					},
					m => m.Say("A z chęcią by można taniej gorzałki wypić."),
					m =>
					{
						m.Say("Ugh...moja głowa...");
						m.Emote("*Wzdycha ciężko*");
					},
					m =>
					{
						m.Say("Pszeciieszzjawcleniepilem...kchanie...noo..wróć...");
						m.Emote("*Wzdycha ze smutkiem*");
					},
					m =>
					{
						m.Say("Portowa mocna...i od razu człowiek szczęśliwy... he he");
						m.Emote("*Śmieje się rubasznie*");
					},
					m => m.Say("A niech to... znów zabraknie mi nia gorzałkę..."),
					m => m.Say("Nie zapijam... Nie chce się truć"),
					m => m.Emote("*Smarka w brudną chustę*"),
					m =>
					{
						m.Say("Kiedyś to było...");
						m.Emote("*Wzdycha ciężko*");
					},
					m => m.Emote("*Poprawia łachmany*"),
					m =>
					{
						m.Say(
							"Będziemy pić! To prosta sprawa! Jesteśmy w Tasandorze a tu nie pić nie wypada!");
						m.Emote("*Wydziera się*");
					},
					m =>
					{
						m.Emote("*Podnosi coś z ziemi*");
						m.Say("Ha! Będzie na kolejkę!");
					},
					m =>
					{
						m.Say("Kurrrrwa... Zajumali mi sakiewkę");
						m.Emote("*Rozgląda się powoli wzdychając*");
					},
				}
			},
			{
				Race.NKrasnolud, new List<Action>
				{
					m => m.Say("Palę, piję i żyje!"),
					m => m.Say("Powiadają, że mój dziad pił prosto z beczki jak z kufla!"),
					m =>
					{
						m.Emote("*Przechyla butlę oblewając całą brodę*");
						m.Say("O żesz kurwa...");
					},
					m =>
					{
						m.Say("Był roz taki tydzień, oko mnie bolało, wypiłech żech se piwko, od razu przestało!");
						m.Emote("*Klaszcze w rytm melodii*");
					},
					m => m.Say(
						"Jo jest spod Aegis, miołech tam kuzyna, borok nie pił piwa, tera chłopa ni ma!"),
					m =>
					{
						m.Say(
							"Jarling nie rozumie nas, gdy pijemy wódkę, Tamael chciałby tak jak my, ale z marnym skutkiem!");
						m.Emote("*Przyśpiewuje do flaszeczki*");
					},
					m => m.Say("To jest Krasnoludzka brać, lubi dużo pić i spać!"),
					m =>
					{
						m.Say("To ma być piwo?! Smakuje jak szczyny trola!");
						m.Emote("*Spluwa na ziemie*");
					},
					m =>
					{
						m.Say("Smoki?! Pierdolone szkodniki... Zatłukbym ale mam piwo do dopicia...");
						m.Emote("*Unosi butlę*");
					},
					m => m.Emote("*Wydobywa z siebie przeraźliwe berknięcie*"),
					m =>
					{
						m.Say("Karzeł?! Jak Ci upierdole nogi sam będziesz karzeł chuju...");
						m.Emote("*Przechyla butlę oblewając się trunkiem*");
					},
					m => m.Emote("*Popala tytoń z fajki*"),
					m => m.Emote("*Odchrząkuje i spluwa gęstą wydzieliną*"),
					m =>
					{
						m.Say("Kiedyś to było...");
						m.Emote("*Wzdycha ciężko*");
					},
					m => m.Emote("*Poprawia brodę wytrzepując z niej okruchy*"),
					m => m.Say("Krasnoludzki browar... tego trzeba w tym zapchlonym mieście"),
					m =>
					{
						m.Emote("*Podnosi coś z ziemi*");
						m.Say("Kurwa... a już myslałem");
					},
					m =>
					{
						m.Say("No i czego sie kurwa gapisz?");
						m.Emote("*Rozgląda się powoli wzdychając*");
					},
				}
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;

		[Constructable]
		public Pijak() : base("- Pijak")
		{
		}

		public override void OnGenderChanged(bool oldFemale)
		{
			base.OnGenderChanged(oldFemale);
			if (Female)
			{
				Title = "- Pijaczka";
			}
			else
			{
				Title = "- Pijak";
			}
		}

		public Pijak(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}

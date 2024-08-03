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
						m.Say("Jest dobrze!");
						m.Emote("*wskazuje kciuk i mówi lekko przepitym głosem*");
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
						m.Say("Jeśli masz ciężki dzień poproś krasnoluda o 'prochy babci'. Obudzisz się dwa dni później, a wszelkie dotychczasowe trudności będą beż znaczenia.");
						m.Emote("*Rechocze*");
					},
					m => m.Say("Wyglądasz na światowca! Prawda to, że kurwy z Garlan goliły się tam na dole?"),
					m => m.Emote("*Smarka w brudną chustę*"),
					m =>
					{
						m.Say("Po wyjściu z więzienia już wiem, że tylko niemyty krasnolud pachnie gorzej od serów C'bulla... A sumie smakuje równie źle ");
						m.Emote("*spluwa*");
					},
					m => m.Emote("*Poprawia łachmany*"),
					m => m.Say("Co mi się tak pić chce? Przecież wczoraj tyle sie piło..."),
					m =>
					{
						m.Emote("*łapie się za głowę*");
						m.Say("Ile ja tych 'prochów babci' wypiłem? ");
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
						m.Say("Jest dobrze!");
						m.Emote("*wskazuje kciuk i mówi lekko przepitym głosem*");
					},
					m =>
					{
						m.Say("Jeśli masz ciężki dzień poproś krasnoluda o 'prochy babci'. Obudzisz się dwa dni później, a wszelkie dotychczasowe trudności będą beż znaczenia.");
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
					m => m.Say("Po wyjściu z więzienia już wiem, że tylko niemyty krasnolud pachnie gorzej od serów C'bulla... A sumie smakuje równie źle "),
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
					m => m.Say("Chcesz to podam ci przepis na prochy babci, musisz tylko podpisać krwią menstruacyjna drowki, o tutaj..."),
					m => m.Say("chwiejac sie na nogach wskzuje na kawalek upapranego pergaminu"),
					m =>
					{
						m.Say("Jest dobrze!");
						m.Emote("*wskazuje kciuk i mówi lekko przepitym głosem*");
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
						m.Say("Po wyjściu z więzienia już wiem, że tylko niemyty krasnolud pachnie gorzej od serów C'bulla... A sumie smakuje równie źle ");
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
			},
			{
				Race.NDrow, new List<Action>
				{
					m => m.Say("Na Loethe... Matrona mi tego nie wybacyz"),
					m => m.Say("chwiejac sie na nogach spoglada na rozbite butelki"),
					m =>
					{
						m.Say("Jest dobrze!");
						m.Emote("*wskazuje kciuk i mówi lekko przepitym głosem*");
					},
					m =>
					{
						m.Say("Pewnego razu Matrona zobaczy jakim jest potezny!");
						m.Emote("*czka*");
					},
				}
				
			},
			{
				Race.NElf, new List<Action>
				{
					m => m.Say("Naneth nie będzie z tego zadowolona..."),
					m => m.Say("A mówił ojciec, że Elfy się nie upijają..."),
					m =>
					{
						m.Say("Jest dobrze!");
						m.Emote("*wskazuje kciuk i mówi lekko przepitym głosem*");
					},
					m =>
					{
						m.Say("Czy to wino na pewno pochodziło z Lotharn? Smakowało jak siki z Tasandory.");
						m.Emote("*spluwa*");
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

#region References

using System.Collections.Generic;

#endregion

namespace Server.Mobiles
{
	public class Mieszczanin : NBaseTalkingNPC
	{
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{ Race.DefaultRace, new List<Action>() },
			{
				Race.NTamael, new List<Action>
				{
					m => m.Say("On i tak nie był tego wart."),
					m => m.Say("Psia krew..."),
					m => m.Say("I co Ci do tego..."),
					m => m.Say("Miłego dnia!"),
					m => m.Say("W najbliższe Sianokosy zjadłbym pieczonego kurczaka... biegał tu kiedyś jakiś."),
					m => m.Say("Ulice wreszcie czyste, jak zimna żytnia!"),
					m => m.Say("Podatki, podatki, więcej podatków. A płaca ta sama."),
					m => m.Say("Ta straż ciągle tu tylko węszy."),
					m => m.Say("Cholera... Znów braknie mi na opłaty..."),
					m => m.Say("Na Bogów... Co to?!"),
					m => m.Say("Najważniejsze, że nic złego się nie stało."),
					m =>
					{
						m.Say("Teraz to dopiero będzie!");
						m.Emote("*uśmiecha się delikatnie*");
					},
					m => m.Say("Jadłem kiedyś w karczmie w Ferion, a później dwie doby spędziłem w wychodku..."),
					m =>
					{
						m.Say("Cholipka...");
						m.Emote("*Rozgląda się powoli wzdychając*");
					},
					m => m.Emote("*Śmieje sie rubasznie*"),
					m => m.Emote("*Śpiewa pieśniczkę rozglądając sie w koło*"),
					m => m.Emote("*Nuci pod nosem*"),
					m => m.Emote("*Gwiżdże machając do kogoś*"),
					m => m.Say("No lepiej nie będzie... Jakoś trzeba żyć..."),
					m => m.Say("Pięknie się żyje... lepiej być nie może."),
					m => m.Say("A kim Ty kurwa jesteś że dajesz mi rade co..?"),
					m =>
					{
						m.Say("Cóż za brak klasy i obycia...");
						m.Emote("*Prycha*");
					},
					m => m.Say("Odejdź... Nie mam teraz czasu"),
				}
			},
			{
				Race.NJarling, new List<Action>
				{
					m =>
					{
						m.Emote("*Zerka w swe kieszenie*");
						m.Say("No i tu też zaczynają wiać... tylko, że pustki");
					},
					m => m.Emote("*Podrzuca mały sztylecik w dłoni*"),
					m => m.Say("Soterios to jednak miał rację... Te nekromanty do niczego się nie nadają..."),
					m => m.Emote("*Patrzy podejrzliwie*"),
					m => m.Say("Parszywe szczury..."),
					m =>
					{
						m.Emote("*Wrzeszczy*");
						m.Say("Rządamy niższych podatków!");
					},
					m => m.Say(
						"Śnieg, mróz i srogie zimy... dziadek mi o nich opowiadał. Geriador usłany był płatkami śniegu."),
					m => m.Emote("*Spluwa*"),
					m => m.Emote("*Donośnie beka*"),
					m =>
					{
						m.Emote("*Uśmiecha się*");
						m.Say("To będzie dobry dzień.");
					},
					m => m.Emote("*Płacze*"),
					m => m.Say("Oby tylko straż się nie interesowała..."),
					m => m.Say("A niech to... więcej nie gram z nimi w kości."),
					m => m.Say("Zjadłoby się zupę w karczmie... wypiłoby się z kamratami... ale nie ma z kim... "),
					m =>
					{
						m.Say("Jak nazywa się kompot z pasem? ....kompas... he he");
						m.Emote("*Śmieje się rubasznie*");
					},
					m => m.Emote("*Drapie się po głowie*"),
					m =>
					{
						m.Emote("*Sięga po sakiewkę na ziemi*");
						m.Say("A to, teraz...należy już do mnie.");
					},
					m => m.Say("Ty Psi synu..."),
					m => m.Emote("*Przeciąga się*"),
					m => m.Emote("*Podrzuca złotą monetę*"),
					m => m.Emote("*Wybucha śmiechem*"),
					m => m.Say(
						"...i wtedy ja mu mówię 'Lepiej się cofnij'... i wiesz co? Cofnął się... Głupi Elf..."),
					m => m.Emote("*Wydmuchuje nos*"),
					m => m.Say(
						"...zioła... zaklęcia ...i.. tłuczek... miecza nie ma na tej liście? Jak ja mam niby walczyć?!"),
				}
			},
			{
				Race.NKrasnolud, new List<Action>
				{
					m =>
					{
						m.Say("Kolejny dzień bez smoków...a to już coś");
						m.Emote("*Uśmiecha sie radnośnie*");
					},
					m => m.Say("Napiłoby sie piwa z kamratami..."),
					m =>
					{
						m.Say("Jest dobrze!");
						m.Emote("*wskazuje kciuk i mówi lekko przepitym głosem*");
					},
					m => m.Say("Jadłem kiedyś w karczmie w Tafroel, a później dwie doby spędziłem w wychodku..."),
					m =>
					{
						m.Say("Krasnal?! Który  to powiedział?! Cholerne wielkoludy!");
						m.Emote("*Drze się na całą okolicę*");
					},
					m =>
					{
						m.Say("Hmm... Pojedzone, tera to by sie coś zjadło");
						m.Emote("*Klepie się po brzuchu*");
					},
					m => m.Emote("*Obraca w palcach złotą monetę*"),
					m => m.Emote("*Kopie w kamienie*"),
					m =>
					{
						m.Say("Smoki?! Gdzie smoki?!");
						m.Emote("*Rozgląda się nerwowo*");
					},
					m =>
					{
						m.Say("Jak sie żyje stary pryku heee?");
						m.Emote("*Śmieje się głośno*");
					},
					m => m.Say("Dej mi spokój, nie mam teraz czasu..."),
					m =>
					{
						m.Say("Dam słowo, że jeszcze wczoraj był pełen");
						m.Emote("*Potrząsa pustym miechem*");
					},
					m => m.Say("Nie zadzieraj... Nie wiesz z kim tańczysz"),
					m =>
					{
						m.Say("Żeby życie miało smaczek... raz wódeczka, raz koniaczek");
						m.Emote("*Otwiera butle przychlając do dna*");
					},
					m => m.Say("Czym by tu się zająć... Eh stara bieda"),
					m => m.Say("Nie chce mi sie gadać... Idź już sobie"),
					m => m.Say("Ci strażnicy to by mogli zacząć łapać kogo trzeba, a nie biednych mieszkańców męczą"),
					m => m.Say(
						"Kumoterstwo to nie tylko piwo i tańce w karczmie..."),
					m => m.Say("A idź do diabła pieprzony tamelski kurwisynu..."),
					m => m.Emote("*Klnie pod nosem*"),
					m =>
					{
						m.Emote("*Grzebie w sakwie*");
						m.Say("Ktoś sie nie bał i zajebał...");
					},
					m => m.Say("Dzień jak codzień... Nic ciekawego"),
					m =>
					{
						m.Say("Pytasz co słychać?");
						m.Emote("*Śmieje się gromko*");
						m.Say("...Stare kurwy nie chcą zdychać");
					},
					m => m.Say("Dobrze, że Relikwia Pana znalazła się w naszej świątyni"),
					m => m.Say("Bez nas... to by te całe rycerzyki z gołymi dupskami biegały!"),
					m => m.Say("Problem... Dobre sobie, w czarnej dupie kurwa jesteśmy a nie mamy problem..."),
					m =>
					{
						m.Emote("*Roztacza wokół zapach górskiego trola*");
						m.Say("Cholerny kapuśniak... ugh");
					}
				}
			},
			{
				Race.NDrow, new List<Action>
				{
					m => m.Say("Dobre czasy nastały. To wszystko dzięki Matronie."),
					m => m.Say("On i tak nie był tego wart."),
					m => m.Say("Na Loethe..."),
					m => m.Say("I co Ci do tego..."),
					m => m.Say("Zabiłbym jakiegoś naziemca."),
					m => m.Say("Ulice wreszcie czyste! Chwała L'Delmah i Noamuth Quortek!"),
					m => m.Say("Chwała L'Delmah, Chwała Loethe!"),
					m => m.Say("Podatki, podatki, więcej podatków. A płaca ta sama."),
					m => m.Say("Ehhhh..."),
					m => m.Say("Cholera... Znów braknie mi na opłaty..."),
					m => m.Say("Na Loethe... Co to?!"),
					m => m.Say("Najważniejsze, że nic złego się nie stało."),
					m => m.Say("Jadłem kiedyś w karczmie w Noamuth Quortek..."),
					m => m.Say("Dzięki nam, Podmrok jest bezpieczny!"),
					m =>
					{
						m.Say("Cholipka...");
						m.Emote("*Rozgląda się powoli wzdychając*");
					},
				}
				
			},
			{
				Race.NElf, new List<Action>
				{
					m => m.Say("Dobre czasy nastały. Naneth niechaj błogosławi tę wyspę."),
					m => m.Say("On i tak nie był tego wart."),
					m => m.Say("Na Naneth..."),
					m => m.Say("I co Ci do tego..."),
					m => m.Say("Miłego dnia, mellon!"),
					m => m.Say("W najbliższe Sianokosy zjadłbym coś z naszych lokalnych upraw..."),
					m => m.Say("Chwała Lotharn, Chwała Naneth!"),
					m => m.Say("Podatki, podatki, więcej podatków. A płaca ta sama."),
					m => m.Say("Ehhhh..."),
					m => m.Say("Cholera... Znów braknie mi na opłaty..."),
					m => m.Say("Na Naneth... Co to?!"),
					m => m.Say("Najważniejsze, że nic złego się nie stało."),
					m =>
					{
						m.Say("Teraz to dopiero będzie!");
						m.Emote("*uśmiecha się delikatnie*");
					},
					m => m.Say("Dzięki Elfom, Lotharn jest bezpieczne!"),
					m =>
					{
						m.Say("Cholipka...");
						m.Emote("*Rozgląda się powoli wzdychając*");
					},
				}
				
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;

		[Constructable]
		public Mieszczanin() : base("- Mieszczanin")
		{
		}

		public override void OnGenderChanged(bool oldFemale)
		{
			base.OnGenderChanged(oldFemale);
			if (Female)
			{
				Title = "- Mieszczanka";
			}
			else
			{
				Title = "- Mieszczanin";
			}
		}

		public Mieszczanin(Serial serial) : base(serial)
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

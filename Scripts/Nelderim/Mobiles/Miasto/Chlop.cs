#region References

using System.Collections.Generic;
using System.Linq;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class Chlop : NBaseTalkingNPC
	{
		private static List<Action> _DefaultActions = new List<Action>
		{
			m => m.Say("Mogłoby troche popadać..."),
			m => m.Say("Oby nie było suszy."),
			m => m.Say("Ah... Mam już dość."),
			m =>
			{
				m.Say(" ...i na jaką cholerę Soterios wypędzał Wyznawców Śmierci i Nekromantów. Przecież oni i tak wrócili");
				m.Emote("*Odgania ręką*");
			},
			m => m.Say("Zjeżdżaj stąd..."),
			m => m.Emote("*Szura powoli nogą po ziemi rozglądając się w koło*"),
			m =>
			{
				m.Say("Trza by załatać dach... znów cieknie.");
				m.Emote("*Wzdycha ponuro*");
			},
			m => m.Say("Kończą sie zapasy, trzaby sie wybrać do Miasta."),
			m => m.Say("Niechaj Naneth pobłogosławi nasze zbiory, bo w tym roku słabo to widzę"),
			m => m.Say("Cholera, trzeba nakarmić kury."),
			m => m.Say("Sprawię sobie chyba nowe buty... Albo chociaż załatam stare."),
			m => m.Say("Oni tak zawsze obiecują... A potem nic z tego nie ma"),
			m => m.Emote("*Rozrzuca coś na ziemi*"),
			m => m.Emote("*Rozgląda się leniwym spojrzeniem*"),
			m => m.Emote("*Przeciąga się*"),
			m => m.Emote("*Nuci pod nosem*"),
			m => m.Emote("*Popija świeże mleko*"),
			m => m.Emote("*Klnie siarczyście pod nosem*"),
			m =>
			{
				m.Say("O kurwa...");
				m.Emote("*Zamyśla się*");
			}
		};

		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{ Race.DefaultRace, _DefaultActions },
			{
				Race.NTamael, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Fron, Rada czy Elbern, Soteriosy, Griffiny i nekromanty... dla mnie jeden chuj... byleby zboże było!"),
					m => m.Say("Oby Matka zesłała urodzaj..."),
					m =>
					{
						m.Say("Tasandorskie ziemie dla Tamaelów!");
						m.Emote("*Prycha*");
					}
				}).ToList()
			},
			{
				Race.NJarling, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Mam juz dość tych śmierdzących Tamaeli..."),
					m => m.Say("Oby Matka zesłała urodzaj..."),
					m =>
					{
						m.Say("Cięzkie to życie na roli...");
						m.Emote("*Wzdycha*");
					}
				}).ToList()
			},
			{
				Race.NKrasnolud, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Chuj z tą władzą... dla mnie nic sie nie zmienia..."),
					m => m.Say("Oby nie było tak źle jak za Soteriosa..."),
					m =>
					{
						m.Say("Co za buc...");
						m.Emote("*Prycha*");
					}
				}).ToList()
			},
			{
				Race.NDrow, new List<Action>
				{
					m => m.Say("Dobre czasy nastały. To wszystko dzięki Matronie."),
					m => m.Say("On i tak nie był tego wart."),
					m => m.Say("Na Loethe..."),
					m => m.Say("I co Ci do tego..."),
					m => m.Say("Podmrok dla Drowów, a reszta won!"),
					m => m.Say("Zabiłbym jakiegoś naziemca."),
					m => m.Say("Podatki, podatki, więcej podatków. A płaca ta sama."),
					m => m.Say("Ehhhh..."),
					m => m.Say("Cholera... Znów braknie mi na opłaty..."),
					m => m.Say("Na Loethe... Co to?!"),
					m => m.Say("Najważniejsze, że nic złego się nie stało."),
					m =>
					{
						m.Say("Chwała Lotharn, Chwała Naneth!.... Eh... jebać Elfy!");
						m.Emote("*śmiesznym głosem naśladuje Elfa w akompaniamencie bardzo zniewieściałych gestów*");
					},
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
					m => m.Say("Ulice wreszcie czyste! Chwała Galadowi!"),
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
					m => m.Say("Jadłem kiedyś w karczmie w Ferion, a później dwie doby spędziłem w wychodku..."),
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
		public Chlop() : base("- Chlop")
		{
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			if (Utility.Random(3) == 0)
			{
				EquipItem(new Pitchfork());
			}
		}

		public Chlop(Serial serial) : base(serial)
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

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
				m.Say("Odejdź... Mam dużo pracy.");
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
			m => m.Say("Muszę jutro zajść do młyna."),
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
					m => m.Say("Fron, Rada czy Elbern... dla mnie jeden chuj..."),
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
					m => m.Say("Oby nie było tak źle jak ostatnio..."),
					m =>
					{
						m.Say("Co za buc...");
						m.Emote("*Prycha*");
					}
				}).ToList()
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

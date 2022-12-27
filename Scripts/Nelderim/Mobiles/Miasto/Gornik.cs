#region References

using System;
using System.Collections.Generic;
using System.Linq;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class Gornik : NBaseTalkingNPC
	{
		private static List<Action> _DefaultActions = new List<Action>
		{
			m => m.Emote("*Ociera pot z czoła*"),
			m => m.Emote("*Przeciera ubrudzoną twarz*"),
			m => m.Emote("*Klnie pod nosem*"),
			m =>
			{
				m.Say("Kurwa... Tyle roboty");
				m.Emote("*Wzdycha ciężko*");
			},
			m =>
			{
				m.Emote("*Odkłada kilof na ziemię po czym wciąga coś do nosa*");
				m.Say("O matulo...");
				m.Emote("*Pociera nos krzywiąc się*");
			},
			m => m.Say("Jak wrócę do domu to zjem konia z kopytami"),
			m => m.Say("Namacham sie kilofem a zarobek z tego marny..."),
			m =>
			{
				m.Say("Chyba mi coś łupnęło w krzyżu...");
				m.Emote("*Łapie sie za plecy*");
			},
			m => m.Say("Ostatnio ponoć zawalił sie strop na paru górników..."),
			m => m.Say("Dobra... Pora na przerwę"),
			m => m.Say("Praca, praca..."),
			m =>
			{
				m.Say("Siedziała na rynku, sprzedawała buty, jak nie miała reszty to dawała dupy!");
				m.Emote("*Przyśpiewuje podczas pracy*");
			},
			m => m.Say("Ciemno jak w dupie..."),
			m =>
			{
				m.Say("Co to za dziewucha w tej różowej halce, chciałem ją posmyrać, obszczała mi palce! ");
				m.Emote("*Przyśpiewuje podczas pracy*");
			},
			m => m.Emote("*Zacharczał wypluwając gęstą wydzieline*"),
			m => m.Say("Kto to widział kurwa..."),
			m => m.Emote("*Spluwa na ziemię*"),
			m => m.Say("Potężne złoże tu wyczuwam..."),
			m => m.Say("Kiedyś się dorobię i sam sobie pałac zbuduje... kiedyś"),
		};
		
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{
				Race.DefaultRace, _DefaultActions
			},
			{
				Race.NTamael, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Aż miło pracować na potęgę wielkiej Tasandory"),
				}).ToList()
			},
			{
				Race.NJarling, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Pierdoleni Tamaelowie... urobek mi podkradają..."),
				}).ToList()
			},
			{
				Race.NKrasnolud, _DefaultActions.Concat(new List<Action>
				{
					m => m.Say("Bez smoków, tak to można pracować..."),
					m => m.Say("Wole pracować z Jarlingami niż z tymi tasandorskimi parchami..."),
				}).ToList()
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;

		[Constructable]
		public Gornik() : base("- Gornik")
		{
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			EquipItem(new Pickaxe());
			
			if (Utility.RandomBool())
			{
				Lantern lantern = new Lantern();
				lantern.Ignite();
				EquipItem(lantern);
			}
		}

		public Gornik(Serial serial) : base(serial)
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

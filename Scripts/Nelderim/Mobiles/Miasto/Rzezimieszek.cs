#region References

using System.Collections.Generic;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class Rzezimieszek : NBaseTalkingNPC
	{
		private static readonly Dictionary<Race, List<Action>> _Actions = new Dictionary<Race, List<Action>>
		{
			{
				Race.DefaultRace, new List<Action>
				{
					m => m.Say("STÓJ! KURWISYNU!"),
					m => m.Say("Potnę Cię szlamo!"),
					m => m.Say("Zabiję Ci matkę!"),
					m => m.Say("Pieniądze albo życie! Haha i tak wezmę oba!"),
					m => m.Say("Zaraz się przekonasz co to dobre rżnięcie!"),
					m => m.Say("Spierdalaj bo zabije!"),
					m => m.Say("Gdzie Ci tak kurwa śpieszno!"),
					m => m.Say("Nikt Cie tu nie usłyszy."),
					m => m.Say("Dawaj co masz albo przetrące Ci łeb!"),
					m => m.Say("Wychędoże Cie w oczodoły!"),
					m => m.Say("Wracaj tu kurwisynu!"),
				}
			}
		};

		protected override Dictionary<Race, List<Action>> NpcActions => _Actions;

		[Constructable]
		public Rzezimieszek() : base("- Rzezimieszek")
		{
			MakeAggressive();
			SetStr(86, 100);
			SetDex(81, 95);
			SetInt(61, 75);

			SetDamage(10, 23);

			SetSkill(SkillName.Fencing, 66.0, 97.5);
			SetSkill(SkillName.Macing, 65.0, 87.5);
			SetSkill(SkillName.MagicResist, 25.0, 47.5);
			SetSkill(SkillName.Swords, 65.0, 87.5);
			SetSkill(SkillName.Tactics, 65.0, 87.5);
			SetSkill(SkillName.Wrestling, 15.0, 37.5);
		}

		public override void InitOutfit()
		{
			base.InitOutfit();

			SetWearable(new Bandana(), GetRandomHue(), 1.0);
			EquipItem(Loot.RandomWeapon());
		}

		public override bool AlwaysMurderer => true;
		public override bool ClickTitle => false;


		public Rzezimieszek(Serial serial) : base(serial)
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

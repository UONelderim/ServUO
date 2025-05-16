#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki starego lodowego smoka")]
	public class StaryLodowySmok : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }

		[Constructable]
		public StaryLodowySmok() : base(AIType.AI_Mage, FightMode.Weakest, 12, 1, 0.2, 0.4)
		{
			Name = "stary lodowy smok";

			Body = 12;
			BaseSoundID = 362;
			Hue = 2124;

			SetStr(896, 925);
			SetDex(96, 115);
			SetInt(476, 495);

			SetHits(578, 695);

			SetDamage(18, 24);

			SetDamageType(ResistanceType.Physical, 30);
			SetDamageType(ResistanceType.Cold, 70);

			SetResistance(ResistanceType.Physical, 55, 75);
			SetResistance(ResistanceType.Fire, 20, 30);
			SetResistance(ResistanceType.Cold, 85, 95);
			SetResistance(ResistanceType.Poison, 25, 45);
			SetResistance(ResistanceType.Energy, 35, 55);

			SetSkill(SkillName.EvalInt, 50.1, 60.0);
			SetSkill(SkillName.Magery, 50.1, 60.0);
			SetSkill(SkillName.MagicResist, 99.1, 100.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 98.5);

			Fame = 18000;
			Karma = -18000;

			VirtualArmor = 60;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.08)
					corpse.DropItem(new BlueDragonsHeart());
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 3);
			AddLoot(LootPack.Gems, 3);
		}

		public override bool AutoDispel => false;
		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 8; } }
		public override int Hides { get { return 15; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override int Scales { get { return 6; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Blue : ScaleType.White); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public StaryLodowySmok(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write(0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}

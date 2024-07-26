#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki starego ognistego smoka")]
	public class StaryOgnistySmok : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }

		[Constructable]
		public StaryOgnistySmok() : base(AIType.AI_Mage, FightMode.Weakest, 12, 1, 0.2, 0.4)
		{
			Name = "stary ognisty smok";

			Body = 49;
			BaseSoundID = 362;
			Hue = 1570;

			SetStr(896, 925);
			SetDex(96, 115);
			SetInt(476, 495);

			SetHits(578, 695);

			SetDamage(18, 24);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Fire, 60);

			SetResistance(ResistanceType.Physical, 55, 75);
			SetResistance(ResistanceType.Fire, 85, 95);
			SetResistance(ResistanceType.Cold, 30, 45);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 40, 50);

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
					corpse.DropItem(new RedDragonsHeart());
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new DragonsBlood());
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new VolcanicAsh());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 3);
			AddLoot(LootPack.Gems, 3);
		}

		public override bool AutoDispel { get { return true; } }
		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 8; } }
		public override int Hides { get { return 15; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 6; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public StaryOgnistySmok(Serial serial) : base(serial)
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

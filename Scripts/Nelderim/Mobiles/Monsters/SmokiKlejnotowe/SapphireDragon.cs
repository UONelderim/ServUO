#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki szafirowego smoka")]
	public class SapphireDragon : Dragon
	{
		[Constructable]
		public SapphireDragon()
		{
			Name = "szafirowy smok";
			BaseSoundID = 362;

			Hue = 1784;

			SetStr(775, 840);
			SetDex(101, 130);
			SetInt(300, 350);
			SetHits(415, 475);
			SetMana(300, 350);
			SetStam(90, 110);

			SetDamage(16, 18);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Energy, 50);

			SetResistance(ResistanceType.Physical, 50, 60);
			SetResistance(ResistanceType.Fire, 30, 45);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 50, 65);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.EvalInt, 90.0, 110.0);
			SetSkill(SkillName.Magery, 90.0, 120.0);
			SetSkill(SkillName.Bushido, 99.1, 110.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 75.1, 100.0);
			SetSkill(SkillName.Meditation, 70.0, 100.0);
			SetSkill(SkillName.Anatomy, 70.0, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 55;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 93;

			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
			SetWeaponAbility(WeaponAbility.TalonStrike);
			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.05)
					corpse.DropItem(new DragonsHeart());
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new DragonsBlood());

				corpse.DropItem(new Sapphire(4));
				corpse.DropItem(new StarSapphire(4));
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 5);
		}

		public override int TreasureMapLevel { get { return 3; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override int Scales { get { return 7; } }
		public override ScaleType ScaleType { get { return ScaleType.Blue; } }
		public override FoodType FavoriteFood { get { return FoodType.Sapphire | FoodType.StarSapphire; } }

		public SapphireDragon(Serial serial) : base(serial)
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

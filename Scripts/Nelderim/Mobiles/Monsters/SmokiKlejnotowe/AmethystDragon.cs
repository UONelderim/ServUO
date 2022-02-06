using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zwloki ametystowego smoka")]
	public class AmethystDragon : Dragon
	{
		[Constructable]
		public AmethystDragon() : base()
		{
			Name = "ametystowy smok";
			BaseSoundID = 362;
			Hue = 1373;
			SetStr(750, 805);
			SetDex(60, 75);
			SetInt(250, 300);
			SetHits(400, 600);
			SetMana(600, 650);
			SetStam(80, 100);

			SetDamage(14, 16);

			SetDamageType(ResistanceType.Physical, 30);
			SetDamageType(ResistanceType.Energy, 70);

			SetResistance(ResistanceType.Physical, 50, 65);
			SetResistance(ResistanceType.Fire, 55, 65);
			SetResistance(ResistanceType.Cold, 45, 60);
			SetResistance(ResistanceType.Poison, 45, 60);
			SetResistance(ResistanceType.Energy, 55, 80);

			SetSkill(SkillName.EvalInt, 90.0, 110.0);
			SetSkill(SkillName.Magery, 90.0, 120.0);
			SetSkill(SkillName.MagicResist, 99.1, 110.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 75.1, 100.0);
			SetSkill(SkillName.Meditation, 70.0, 100.0);
			SetSkill(SkillName.Anatomy, 70.0, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 91;

			SetWeaponAbility(WeaponAbility.ForceOfNature);
			SetWeaponAbility(WeaponAbility.DefenseMastery);
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

				corpse.DropItem(new Amethyst(8));
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
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 7; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Yellow : ScaleType.Black); } }
		public override FoodType FavoriteFood { get { return FoodType.Amethyst; } }

		public AmethystDragon(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}

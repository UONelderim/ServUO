#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki rubinowego smoka")]
	public class RubyDragon : Dragon
	{
		[Constructable]
		public RubyDragon()
		{
			Name = "rubinowy smok";
			BaseSoundID = 362;
			Hue = 1157;
			SetStr(825, 870);
			SetDex(80, 95);
			SetInt(325, 350);
			SetHits(450, 520);
			SetMana(350, 400);
			SetStam(110, 125);

			SetDamage(20, 22);

			SetDamageType(ResistanceType.Physical, 25);
			SetDamageType(ResistanceType.Fire, 75);

			SetResistance(ResistanceType.Physical, 60, 70);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.EvalInt, 90.0, 110.0);
			SetSkill(SkillName.Magery, 90.0, 120.0);
			SetSkill(SkillName.MagicResist, 99.1, 110.0);
			SetSkill(SkillName.Tactics, 90.1, 100.0);
			SetSkill(SkillName.Wrestling, 75.1, 100.0);
			SetSkill(SkillName.Meditation, 70.0, 100.0);
			SetSkill(SkillName.Anatomy, 70.0, 100.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 65;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 94;

			SetWeaponAbility(WeaponAbility.MortalStrike);
			SetWeaponAbility(WeaponAbility.CrushingBlow);
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

				corpse.DropItem(new Ruby(8));
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 5);
		}

		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 10; } }
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 7; } }
		public override ScaleType ScaleType { get { return ScaleType.Red; } }
		public override FoodType FavoriteFood { get { return FoodType.Ruby; } }

		public RubyDragon(Serial serial) : base(serial)
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

#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki starozytnego lodowego smoka")]
	public class NStarozytnyLodowySmok : BaseCreature
	{
		public override bool BardImmune { get { return true; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override double DispelDifficulty { get { return 135.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool AutoDispel { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 19; } }
		public override int Hides { get { return 60; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override int Scales { get { return 20; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		[Constructable]
		public NStarozytnyLodowySmok() : base(AIType.AI_Boss, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "starozytny lodowy smok";

			Body = 46;
			BaseSoundID = 362;
			Hue = 2906;

			SetStr(1400, 1500);
			SetDex(110, 120);
			SetInt(750, 850);

			SetHits(20000);

			SetDamage(20, 38);

			SetDamageType(ResistanceType.Physical, 0);
			SetDamageType(ResistanceType.Cold, 80);
			SetDamageType(ResistanceType.Energy, 20);

			SetResistance(ResistanceType.Physical, 75, 85);
			SetResistance(ResistanceType.Fire, 40, 60);
			SetResistance(ResistanceType.Cold, 90, 100);
			SetResistance(ResistanceType.Poison, 50, 60);
			SetResistance(ResistanceType.Energy, 60, 70);

			SetSkill(SkillName.EvalInt, 120.1, 130.0);
			SetSkill(SkillName.Magery, 120.1, 130.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Tactics, 110.1, 120.0);
			SetSkill(SkillName.Wrestling, 110.1, 120.0);

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 70;
			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.15)
					corpse.DropItem(new BlueDragonsHeart());
				if (Utility.RandomDouble() < 0.30)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}


		public NStarozytnyLodowySmok(Serial serial) : base(serial)
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

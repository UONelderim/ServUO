#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki starozytnego ognistego smoka")]
	public class NStarozytnySmok : BaseCreature
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
		public override HideType HideType { get { return HideType.Horned; } }
		public override int Scales { get { return 20; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		[Constructable]
		public NStarozytnySmok() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.25, 0.5)
		{
			Name = "Starozytny Ognisty Smok";

			Body = 46;
			BaseSoundID = 362;
			Hue = 1570;

			SetStr(1500, 1600);
			SetDex(120, 130);
			SetInt(600, 800);

			SetHits(20000);

			SetDamage(25, 35);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Fire, 80);

			SetResistance(ResistanceType.Physical, 70, 75);
			SetResistance(ResistanceType.Fire, 90, 100);
			SetResistance(ResistanceType.Cold, 35, 60);
			SetResistance(ResistanceType.Poison, 40, 55);
			SetResistance(ResistanceType.Energy, 50, 60);

			SetSkill(SkillName.EvalInt, 110.1, 120.0);
			SetSkill(SkillName.Magery, 110.1, 120.0);
			SetSkill(SkillName.MagicResist, 110.1, 120.0);
			SetSkill(SkillName.Tactics, 110.1, 120.0);
			SetSkill(SkillName.Wrestling, 110.1, 120.0);

			Fame = 25000;
			Karma = -25000;

			VirtualArmor = 70;

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
					corpse.DropItem(new RedDragonsHeart());
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new DragonsBlood());
				if (Utility.RandomDouble() < 0.30)
					corpse.DropItem(new VolcanicAsh());
			}

			base.OnCarve(from, corpse, with);
		}


		public NStarozytnySmok(Serial serial) : base(serial)
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

#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki smoczyska")]
	public class NelderimDragon : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }

		[Constructable]
		public NelderimDragon() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.2, 0.4)
		{
			Name = NameList.RandomName("dragon");

			Body = Utility.RandomList(12, 59);
			BaseSoundID = 362;
			Hue = 1034;

			SetStr(1196, 1225);
			SetDex(200, 250);
			SetInt(550, 600);

			SetHits(1500, 1600);

			SetDamage(35, 45);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 60, 75);
			SetResistance(ResistanceType.Fire, 70, 80);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 45, 50);
			SetResistance(ResistanceType.Energy, 45, 50);

			SetSkill(SkillName.EvalInt, 99.1, 120.0);
			SetSkill(SkillName.Magery, 99.1, 120.0);
			SetSkill(SkillName.MagicResist, 100.1, 120.0);
			SetSkill(SkillName.Tactics, 100.1, 120.0);
			SetSkill(SkillName.Wrestling, 100.1, 120.0);

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 60;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.30)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override bool OnBeforeDeath()
		{
			AddLoot(LootPack.ClericScrolls);
			return base.OnBeforeDeath();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 2);
			AddLoot(LootPack.Gems, 5);
			AddLoot(LootPack.MageryRegs, 30);
		}

		public override bool AutoDispel { get { return true; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 8; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override int Scales { get { return 5; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Yellow : ScaleType.Red); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public NelderimDragon(Serial serial) : base(serial)
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

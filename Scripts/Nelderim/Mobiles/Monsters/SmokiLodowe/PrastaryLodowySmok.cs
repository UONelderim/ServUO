#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki prastarego lodowego smoka")]
	public class PrastaryLodowySmok : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.20; } }

		[Constructable]
		public PrastaryLodowySmok() : base(AIType.AI_Mage, FightMode.Closest, 11, 1, 0.2, 0.4)
		{
			Name = "prastary lodowy smok";

			Body = 46;
			BaseSoundID = 362;
			Hue = 1080;

			SetStr(1096, 1125);
			SetDex(116, 135);
			SetInt(536, 575);

			SetHits(878, 995);

			SetDamage(25, 35);

			SetDamageType(ResistanceType.Physical, 20);
			SetDamageType(ResistanceType.Cold, 80);

			SetResistance(ResistanceType.Physical, 60, 75);
			SetResistance(ResistanceType.Fire, 35, 50);
			SetResistance(ResistanceType.Cold, 90, 100);
			SetResistance(ResistanceType.Poison, 45, 50);
			SetResistance(ResistanceType.Energy, 45, 50);

			SetSkill(SkillName.EvalInt, 80.1, 100.0);
			SetSkill(SkillName.Magery, 90.1, 120.0);
			SetSkill(SkillName.MagicResist, 100.1, 120.0);
			SetSkill(SkillName.Tactics, 100.1, 120.0);
			SetSkill(SkillName.Wrestling, 100.1, 120.0);

			Fame = 20000;
			Karma = -20000;

			VirtualArmor = 65;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new BlueDragonsHeart());
				if (Utility.RandomDouble() < 0.20)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich);
			AddLoot(LootPack.Gems, 4);
		}

		public override bool AutoDispel { get { return true; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override int Meat { get { return 10; } }
		public override int Hides { get { return 20; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override int Scales { get { return 8; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Blue : ScaleType.White); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public PrastaryLodowySmok(Serial serial) : base(serial)
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

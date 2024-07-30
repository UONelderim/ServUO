#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki lodowego smoka")]
	public class LodowySmok : BaseCreature
	{
		[Constructable]
		public LodowySmok() : base(AIType.AI_Mage, FightMode.Weakest, 12, 1, 0.2, 0.4)
		{
			Name = "lodowy smok";

			Body = 12;
			BaseSoundID = 362;
			Hue = 1080;

			SetStr(796, 825);
			SetDex(86, 105);
			SetInt(436, 475);

			SetHits(478, 495);

			SetDamage(14, 20);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Cold, 60);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 20, 30);
			SetResistance(ResistanceType.Cold, 80, 90);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 35, 45);

			SetSkill(SkillName.EvalInt, 30.1, 40.0);
			SetSkill(SkillName.Magery, 30.1, 40.0);
			SetSkill(SkillName.MagicResist, 99.1, 100.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 92.5);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 60;

			Tamable = true;
			ControlSlots = 3;
			MinTameSkill = 96.9;

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.05)
					corpse.DropItem(new BlueDragonsHeart());
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new DragonsBlood());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 2);
			// 07.01.2013 :: szczaw :: usuniecie PackGold
			//PackGold(100, 300 );
			AddLoot(LootPack.Gems, 2);
		}

		public override double AttackMasterChance { get { return 0.25; } }
		public override bool AutoDispel { get { return true; } }
		public override int TreasureMapLevel { get { return 4; } }
		public override int Meat { get { return 8; } }
		public override int Hides { get { return 12; } }
		public override HideType HideType { get { return HideType.Spined; } }
		public override int Scales { get { return 5; } }
		public override ScaleType ScaleType { get { return (Body == 12 ? ScaleType.Blue : ScaleType.White); } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }

		public LodowySmok(Serial serial) : base(serial)
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

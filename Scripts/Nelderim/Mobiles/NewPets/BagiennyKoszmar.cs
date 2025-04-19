#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki bagiennego koszmara")]
	public class BagiennyKoszmar : BaseMount
	{
		[Constructable]
		public BagiennyKoszmar() : this("bagienny koszmar")
		{
		}

		[Constructable]
		public BagiennyKoszmar(string name) : base(name, 0x74, 0x3EA7, AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2,
			0.4)
		{
			BaseSoundID = 0xA8;

			SetStr(496, 525);
			SetDex(86, 105);
			SetInt(86, 125);

			SetHits(298, 315);

			SetDamage(16, 22);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Poison, 40);
			SetDamageType(ResistanceType.Energy, 20);

			SetResistance(ResistanceType.Physical, 55, 65);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 60, 70);
			SetResistance(ResistanceType.Energy, 20, 30);

			SetSkill(SkillName.EvalInt, 10.4, 50.0);
			SetSkill(SkillName.Magery, 10.4, 50.0);
			SetSkill(SkillName.MagicResist, 85.3, 100.0);
			SetSkill(SkillName.Tactics, 97.6, 100.0);
			SetSkill(SkillName.Wrestling, 80.5, 92.5);
			SetSkill(SkillName.Poisoning, 50.0, 90.0);

			Fame = 14000;
			Karma = -14000;

			VirtualArmor = 60;

			Tamable = true;
			ControlSlots = 2;
			MinTameSkill = 96.1;

			switch (Utility.Random(3))
			{
				case 0:
				{
					BodyValue = 116;
					ItemID = 16039;
					Hue = 1388;
					break;
				}
				case 1:
				{
					BodyValue = 178;
					ItemID = 16041;
					Hue = 1389;
					break;
				}
				case 2:
				{
					BodyValue = 179;
					ItemID = 16055;
					Hue = 1390;
					break;
				}
			}

			PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 5)));

			SetSpecialAbility(SpecialAbility.DragonBreath);
		}

		public override void OnCarve(Mobile from, Corpse corpse, Item with)
		{
			if (!IsBonded && !corpse.Carved && !IsChampionSpawn)
			{
				if (Utility.RandomDouble() < 0.10)
					corpse.DropItem(new BowstringNightmare());
			}

			base.OnCarve(from, corpse, with);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
			AddLoot(LootPack.Average);
			AddLoot(LootPack.LowScrolls);
			AddLoot(LootPack.Potions);
		}

		public override int GetAngerSound()
		{
			if (!Controlled)
				return 0x16A;

			return base.GetAngerSound();
		}

		public override Poison HitPoison { get { return Poison.Greater; } }
		public override int Meat { get { return 3; } }
		public override int Hides { get { return 5; } }
		public override HideType HideType { get { return HideType.Barbed; } }
		public override FoodType FavoriteFood { get { return FoodType.Meat; } }
		public override bool CanAngerOnTame { get { return true; } }
		
		public override PackInstinct PackInstinct => PackInstinct.Daemon;

		public BagiennyKoszmar(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			if (BaseSoundID == 0x16A)
				BaseSoundID = 0xA8;
		}
	}
}

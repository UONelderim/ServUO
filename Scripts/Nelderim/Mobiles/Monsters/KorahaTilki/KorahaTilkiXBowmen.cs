#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class KorahaTilkiXBowmen : BaseCreature
	{
		[Constructable]
		public KorahaTilkiXBowmen() : base(AIType.AI_Archer, FightMode.Closest, 12, 5, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Tilki Kusznik";
			Hue = Race.RandomSkinHue();

			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
			}


			SetStr(150, 180);
			SetDex(140, 160);
			SetInt(60, 80);

			SetHits(180, 210);

			SetDamage(14, 18);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 35, 50);
			SetResistance(ResistanceType.Fire, 55, 65);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 40, 45);
			SetResistance(ResistanceType.Energy, 40, 45);

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Archery, 100.0, 120.0);
			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 70.5, 90.0);
			SetSkill(SkillName.Swords, 60.0, 82.5);
			SetSkill(SkillName.Tactics, 90.0, 110.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			Item hair = new Item(Utility.RandomList(0x203C));
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			LeatherChest chest = new LeatherChest();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem(chest);

			LeatherGloves Gloves = new LeatherGloves();
			Gloves.Hue = 1319;
			Gloves.Movable = false;
			EquipItem(Gloves);

			LeatherLegs legs = new LeatherLegs();
			legs.Hue = 1319;
			legs.Movable = false;
			EquipItem(legs);

			Sandals Boot = new Sandals();
			Boot.Hue = 48;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 48;
			Cloa.Movable = false;
			AddItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 48;
			sash.Movable = false;
			EquipItem(sash);

			Bandana band = new Bandana();
			band.Hue = 48;
			band.Movable = false;
			AddItem(band);

			RepeatingCrossbow bow = new RepeatingCrossbow();
			bow.Movable = false;
			bow.Hue = 1437;
			bow.Movable = false;
			AddItem(bow);

			PackItem(new Bolt(Utility.Random(30, 35)));

			SetWeaponAbility(WeaponAbility.DoubleStrike);
		}

		public override double WeaponAbilityChance => 0.3;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Meager);
			if (Utility.RandomDouble() < 0.3)
				PackItem(new BowstringCannabis());
		}

		public override double AttackMasterChance { get { return 0.25; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool AutoDispel => false;
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }


		public KorahaTilkiXBowmen(Serial serial) : base(serial)
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

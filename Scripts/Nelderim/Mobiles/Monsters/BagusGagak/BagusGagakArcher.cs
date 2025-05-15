#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class BagusGagakArcher : BaseCreature
	{
		[Constructable]
		public BagusGagakArcher() : base(AIType.AI_Archer, FightMode.Closest, 12, 5, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Gagak Lucznik";
			Hue = Race.RandomSkinHue();

			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
				AddItem(new Kilt(Utility.RandomNeutralHue()));
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
				AddItem(new ShortPants(Utility.RandomNeutralHue()));
			}


			SetStr(130, 160);
			SetDex(140, 160);
			SetInt(55, 70);

			SetHits(160, 190);

			SetDamage(17, 26);

			SetDamageType(ResistanceType.Physical, 80);
			SetDamageType(ResistanceType.Poison, 20);

			SetResistance(ResistanceType.Physical, 35, 40);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Archery, 100.0, 120.0);
			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 70.0, 90.0);
			SetSkill(SkillName.Swords, 60.0, 82.5);
			SetSkill(SkillName.Tactics, 100.0, 110.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 45;

			SetWeaponAbility(WeaponAbility.ParalyzingBlow);

			Item hair = new Item(Utility.RandomList(0x203C));
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			LeatherChest chest = new LeatherChest();
			chest.Hue = 2101;
			chest.Movable = false;
			EquipItem(chest);

			LeatherGloves Gloves = new LeatherGloves();
			Gloves.Hue = 2101;
			Gloves.Movable = false;
			EquipItem(Gloves);

			LeatherLegs legs = new LeatherLegs();
			legs.Hue = 2101;
			legs.Movable = false;
			EquipItem(legs);

			Boots Boot = new Boots();
			Boot.Hue = 262;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 262;
			Cloa.Movable = false;
			AddItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 262;
			sash.Movable = false;
			EquipItem(sash);

			Bandana band = new Bandana();
			band.Hue = 2101;
			band.Movable = false;
			AddItem(band);

			CompositeBow bow = new CompositeBow();
			bow.Movable = false;
			bow.Hue = 424;
			bow.Movable = false;

			AddItem(bow);

			PackItem(new Arrow(Utility.Random(30, 35)));
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


		public BagusGagakArcher(Serial serial) : base(serial)
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

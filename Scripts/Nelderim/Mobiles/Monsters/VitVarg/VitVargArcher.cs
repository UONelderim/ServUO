#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class VitVargArcher : BaseCreature
	{
		[Constructable]
		public VitVargArcher() : base(AIType.AI_Archer, FightMode.Closest, 12, 5, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Varg Lucznik";
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


			SetStr(120, 150);
			SetDex(140, 160);
			SetInt(60, 80);

			SetHits(150, 190);

			SetDamage(18, 28);

			SetDamageType(ResistanceType.Physical, 70);
			SetDamageType(ResistanceType.Cold, 30);

			SetResistance(ResistanceType.Physical, 35, 50);
			SetResistance(ResistanceType.Fire, 25, 40);
			SetResistance(ResistanceType.Cold, 55, 65);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Archery, 100.0, 120.0);
			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 57.5, 80.0);
			SetSkill(SkillName.Swords, 60.0, 82.5);
			SetSkill(SkillName.Tactics, 90.0, 110.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 45;

			Item hair = new Item(Utility.RandomList(0x203C));
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			LeatherChest chest = new LeatherChest();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem(chest);

			LeatherGloves Gloves = new LeatherGloves();
			Gloves.Hue = 856;
			Gloves.Movable = false;
			EquipItem(Gloves);

			LeatherLegs legs = new LeatherLegs();
			legs.Hue = 856;
			legs.Movable = false;
			EquipItem(legs);

			Boots Boot = new Boots();
			Boot.Hue = 856;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem(sash);

			Bandana band = new Bandana();
			band.Hue = 288;
			band.Movable = false;
			AddItem(band);

			Bow bow = new Bow();
			bow.Movable = false;
			bow.Hue = 781;
			bow.Movable = false;

			AddItem(bow);

			PackItem(new Arrow(Utility.Random(25, 30)));

			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
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


		public VitVargArcher(Serial serial) : base(serial)
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

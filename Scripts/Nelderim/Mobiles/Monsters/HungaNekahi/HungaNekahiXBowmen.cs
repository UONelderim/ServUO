#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class HungaNekahiXBowmen : BaseCreature
	{
		[Constructable]
		public HungaNekahiXBowmen() : base(AIType.AI_Archer, FightMode.Closest, 12, 5, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Nekahi Kusznik";
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


			SetStr(180, 200);
			SetDex(120, 140);
			SetInt(40, 60);

			SetHits(200, 240);

			SetDamage(22, 28);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Energy, 40);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 35, 40);
			SetResistance(ResistanceType.Cold, 35, 40);
			SetResistance(ResistanceType.Poison, 35, 40);
			SetResistance(ResistanceType.Energy, 55, 60);

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Archery, 100.0, 110.0);
			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 80.5, 90.0);
			SetSkill(SkillName.Swords, 60.0, 82.5);
			SetSkill(SkillName.Tactics, 90.0, 110.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 38;

			Item hair = new Item(Utility.RandomList(0x203C));
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			LeatherChest chest = new LeatherChest();
			chest.Hue = 556;
			chest.Movable = false;
			EquipItem(chest);

			LeatherGloves Gloves = new LeatherGloves();
			Gloves.Hue = 556;
			Gloves.Movable = false;
			EquipItem(Gloves);

			LeatherLegs legs = new LeatherLegs();
			legs.Hue = 556;
			legs.Movable = false;
			EquipItem(legs);

			Boots Boot = new Boots();
			Boot.Hue = 33;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 33;
			Cloa.Movable = false;
			AddItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 33;
			sash.Movable = false;
			EquipItem(sash);

			Bandana band = new Bandana();
			band.Hue = 33;
			band.Movable = false;
			AddItem(band);

			HeavyCrossbow bow = new HeavyCrossbow();
			bow.Movable = false;
			bow.Hue = 589;
			bow.Movable = false;
			AddItem(bow);

			PackItem(new Bolt(Utility.Random(30, 35)));

			SetWeaponAbility(WeaponAbility.Dismount);
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


		public HungaNekahiXBowmen(Serial serial) : base(serial)
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

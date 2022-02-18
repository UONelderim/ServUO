#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class KorahaTilkiWarrior : BaseCreature
	{
		[Constructable]
		public KorahaTilkiWarrior() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Tilki Wojownik";
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

			SetStr(200, 240);
			SetDex(100, 140);
			SetInt(50, 65);

			SetHits(250, 300);

			SetDamage(12, 15);


			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 35, 50);
			SetResistance(ResistanceType.Fire, 55, 65);
			SetResistance(ResistanceType.Cold, 45, 50);
			SetResistance(ResistanceType.Poison, 40, 55);
			SetResistance(ResistanceType.Energy, 40, 55);


			SetSkill(SkillName.Anatomy, 120.0);
			SetSkill(SkillName.Fencing, 50.0, 70.0);
			SetSkill(SkillName.Macing, 50.0, 70.0);
			SetSkill(SkillName.MagicResist, 95.0, 100.0);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.Lumberjacking, 120.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 45;

			LeatherCap Helm = new LeatherCap();
			Helm.Hue = 1319;
			Helm.Movable = false;
			EquipItem(Helm);

			LeatherChest chest = new LeatherChest();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem(chest);

			LeatherGorget Gorget = new LeatherGorget();
			Gorget.Hue = 1319;
			Gorget.Movable = false;
			EquipItem(Gorget);

			LeatherGloves Gloves = new LeatherGloves();
			Gloves.Hue = 1319;
			Gloves.Movable = false;
			EquipItem(Gloves);

			LeatherLegs legs = new LeatherLegs();
			legs.Hue = 1319;
			legs.Movable = false;
			EquipItem(legs);

			Boots Boot = new Boots();
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

			Scimitar sword = new Scimitar();
			sword.Hue = 1437;
			sword.Movable = false;
			EquipItem(sword);

			BronzeShield Shield = new BronzeShield();
			Shield.Hue = 1437;
			Shield.Movable = false;
			EquipItem(Shield);

			SetWeaponAbility(WeaponAbility.DoubleStrike);
		}

		public override double WeaponAbilityChance => 0.3;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }


		public KorahaTilkiWarrior(Serial serial) : base(serial)
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

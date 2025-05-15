#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class KorahaTilkiLord : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.2; } }

		[Constructable]
		public KorahaTilkiLord() : base(AIType.AI_Melee, FightMode.Strongest, 12, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

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

			SetStr(330, 380);
			SetDex(150, 200);
			SetInt(100, 120);

			SetHits(400, 450);

			SetDamage(18, 24);


			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 60, 70);
			SetResistance(ResistanceType.Cold, 35, 45);
			SetResistance(ResistanceType.Poison, 45, 60);
			SetResistance(ResistanceType.Energy, 45, 60);


			SetSkill(SkillName.Anatomy, 120.0);
			SetSkill(SkillName.Fencing, 50.0, 70.0);
			SetSkill(SkillName.Macing, 50.0, 70.0);
			SetSkill(SkillName.MagicResist, 100.0, 120.0);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.Lumberjacking, 120.0);

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 50;

			PlateHelm Helm = new PlateHelm();
			Helm.Hue = 1319;
			Helm.Movable = false;
			EquipItem(Helm);

			PlateChest chest = new PlateChest();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem(chest);

			PlateArms arms = new PlateArms();
			arms.Hue = 1319;
			arms.Movable = false;
			EquipItem(arms);

			PlateGorget Gorget = new PlateGorget();
			Gorget.Hue = 1319;
			Gorget.Movable = false;
			EquipItem(Gorget);

			PlateGloves Gloves = new PlateGloves();
			Gloves.Hue = 1319;
			Gloves.Movable = false;
			EquipItem(Gloves);

			PlateLegs legs = new PlateLegs();
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

			LargeBattleAxe axe = new LargeBattleAxe();
			axe.Hue = 1437;
			axe.Movable = false;
			EquipItem(axe);

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override double WeaponAbilityChance => 0.3;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
			AddLoot(LootPack.Gems, 1);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel => false;
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }


		public KorahaTilkiLord(Serial serial) : base(serial)
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

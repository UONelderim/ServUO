#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class BagusGagakLord : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.2; } }

		[Constructable]
		public BagusGagakLord() : base(AIType.AI_Melee, FightMode.Strongest, 12, 1, 0.2, 0.4)
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
			SetSkill(SkillName.Lumberjacking, 100.0);

			Fame = 12000;
			Karma = -12000;

			VirtualArmor = 50;

			SetWeaponAbility(WeaponAbility.MortalStrike);

			BearMask Helm = new BearMask();
			Helm.Hue = 2101;
			Helm.Movable = false;
			EquipItem(Helm);

			BoneChest chest = new BoneChest();
			chest.Hue = 2101;
			chest.Movable = false;
			EquipItem(chest);

			BoneArms arms = new BoneArms();
			arms.Hue = 2101;
			arms.Movable = false;
			EquipItem(arms);

			BoneGloves Gloves = new BoneGloves();
			Gloves.Hue = 2101;
			Gloves.Movable = false;
			EquipItem(Gloves);

			BoneLegs legs = new BoneLegs();
			legs.Hue = 2101;
			legs.Movable = false;
			EquipItem(legs);

			FurBoots Boot = new FurBoots();
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

			CrescentBlade sword = new CrescentBlade();
			sword.Hue = 424;
			sword.Movable = false;
			EquipItem(sword);
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


		public BagusGagakLord(Serial serial) : base(serial)
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

#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class KorahaTilkiPikador : BaseCreature
	{
		[Constructable]
		public KorahaTilkiPikador() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.1, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Tilki Pikador";
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


			SetStr(320, 420);
			SetDex(90, 140);
			SetInt(100, 160);
			SetHits(240, 320);

			SetDamage(14, 18);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 35, 45);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);


			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 80.0, 100.0);
			SetSkill(SkillName.Swords, 90.0, 90.0);
			SetSkill(SkillName.Tactics, 90.0, 110.0);
			SetSkill(SkillName.Anatomy, 90.0, 105.2);
			SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill(SkillName.Fencing, 100.0, 120.0);

			Fame = 6000;
			Karma = -6000;

			VirtualArmor = 45;

			StandardPlateKabuto Helm = new StandardPlateKabuto();
			Helm.Hue = 1319;
			Helm.Movable = false;
			EquipItem(Helm);

			RingmailChest chest = new RingmailChest();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem(chest);

			RingmailGloves Gloves = new RingmailGloves();
			Gloves.Hue = 1319;
			Gloves.Movable = false;
			EquipItem(Gloves);

			RingmailLegs legs = new RingmailLegs();
			legs.Hue = 1319;
			legs.Movable = false;
			EquipItem(legs);

			Boots Boot = new Boots();
			Boot.Hue = 48;
			EquipItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 48;
			EquipItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 48;
			sash.Movable = false;
			EquipItem(sash);

			ShortSpear sword = new ShortSpear();
			sword.Hue = 1437;
			sword.Movable = false;
			EquipItem(sword);

			Horse mount = new Horse();

			mount.ControlMaster = this;
			mount.Controlled = true;
			mount.InvalidateProperties();

			mount.Rider = this;

			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override double WeaponAbilityChance => 0.3;

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( Mount != null )
				Mount.Rider = null;

			return base.OnBeforeDeath();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
		}

		public override double AttackMasterChance { get { return 0.10; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }


		public KorahaTilkiPikador(Serial serial) : base(serial)
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

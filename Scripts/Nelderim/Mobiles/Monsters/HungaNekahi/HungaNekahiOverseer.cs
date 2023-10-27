#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class HungaNekahiOverseer : BaseCreature
	{
		[Constructable]
		public HungaNekahiOverseer() : base(AIType.AI_Melee, FightMode.Weakest, 14, 2, 0.15, 0.3)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Nekahi Nadzorca";
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

			SetStr(300, 340);
			SetDex(100, 150);
			SetInt(100, 140);
			SetHits(420, 460);

			SetDamage(20, 28);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 40, 45);
			SetResistance(ResistanceType.Cold, 40, 45);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 30, 40);


			SetSkill(SkillName.MagicResist, 57.5, 80.0);
			SetSkill(SkillName.Macing, 90.0, 120.0);
			SetSkill(SkillName.Tactics, 90.0, 102.5);
			SetSkill(SkillName.Anatomy, 90.0, 105.2);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			EquipItem(new BoneHelm { Hue = 556, Movable = false });
			EquipItem(new BoneChest { Hue = 556, Movable = false });
			EquipItem(new BoneGloves { Hue = 556, Movable = false });
			EquipItem(new BoneLegs { Hue = 556, Movable = false });
			EquipItem(new ThighBoots { Hue = 33 });
			EquipItem(new Cloak { Hue = 33, Movable = false });
			EquipItem(new BodySash { Hue = 33, Movable = false });
			EquipItem(new HammerPick { Hue = 589, Movable = false });

			SetWeaponAbility(WeaponAbility.ArmorIgnore);
		}

		public override double WeaponAbilityChance => 0.3;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich);
		}

		public override double AttackMasterChance { get { return 0.10; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }


		public HungaNekahiOverseer(Serial serial) : base(serial)
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

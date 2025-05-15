#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class VitVargMage : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.2; } }
		public override double SwitchTargetChance { get { return 0.2; } }

		[Constructable]
		public VitVargMage() : base(AIType.AI_Mage, FightMode.Weakest, 12, 6, 0.2, 0.4)
		{
			Title = "- Varg Mag";
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


			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem(Cloa);

			Sandals Boot = new Sandals();
			Boot.Hue = 1109;
			AddItem(Boot);

			FemaleElvenRobe rob = new FemaleElvenRobe();
			rob.Hue = 856;
			AddItem(rob);
			rob.Movable = false;

			BlackStaff staff = new BlackStaff();
			staff.Attributes.SpellChanneling = 1;
			AddItem(staff);
			staff.Movable = false;

			SetStr(250, 300);
			SetDex(80, 120);
			SetInt(300, 350);

			SetHits(260, 320);

			SetDamage(10, 12);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 50);
			SetResistance(ResistanceType.Fire, 30, 45);
			SetResistance(ResistanceType.Cold, 50, 55);
			SetResistance(ResistanceType.Poison, 40, 50);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.EvalInt, 90.0, 110.0);
			SetSkill(SkillName.Magery, 95.1, 110.0);
			SetSkill(SkillName.Meditation, 80.5, 90.0);
			SetSkill(SkillName.MagicResist, 85.0, 110.0);
			SetSkill(SkillName.Tactics, 90.0, 100.0);
			SetSkill(SkillName.Wrestling, 90.0, 100.0);

			Fame = 8000;
			Karma = -8000;

			VirtualArmor = 30;

			Item hair = new Item(Utility.RandomList(0x203C));
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);
		}


		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 2);
			AddLoot(LootPack.Gems, 1);
			AddLoot(LootPack.MageryRegs, 15, 30);
		}

		public override bool ShowFameTitle { get { return true; } }
		public override bool AutoDispel => false;
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override int TreasureMapLevel { get { return 1; } }

		public VitVargMage(Serial serial) : base(serial)
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

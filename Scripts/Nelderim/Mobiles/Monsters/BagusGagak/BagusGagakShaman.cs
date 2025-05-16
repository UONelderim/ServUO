#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class BagusGagakShaman : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }

		[Constructable]
		public BagusGagakShaman() : base(AIType.AI_Mage, FightMode.Closest, 12, 6, 0.2, 0.4)
		{
			Title = "- Gagak Szaman";
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
			Cloa.Hue = 262;
			AddItem(Cloa);

			Sandals Boot = new Sandals();
			Boot.Hue = 262;
			AddItem(Boot);

			LongPants pant = new LongPants();
			pant.Hue = 2101;
			pant.Movable = false;
			AddItem(pant);

			FancyShirt shirt = new FancyShirt();
			shirt.Hue = 2101;
			shirt.Movable = false;
			AddItem(shirt);

			Doublet doub = new Doublet();
			doub.Hue = 2101;
			doub.Movable = false;
			AddItem(doub);

			Robe rob = new Robe();
			rob.Hue = 2101;
			rob.Movable = false;
			AddItem(rob);

			AddItem(new GnarledStaff());

			SetStr(400, 450);
			SetDex(101, 140);
			SetInt(450, 600);

			SetHits(280, 340);

			SetDamage(5, 9);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 40);
			SetResistance(ResistanceType.Fire, 30, 45);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 45, 55);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.EvalInt, 80.2, 110.0);
			SetSkill(SkillName.Magery, 95.1, 110.0);
			SetSkill(SkillName.Meditation, 27.5, 50.0);
			SetSkill(SkillName.MagicResist, 77.5, 100.0);
			SetSkill(SkillName.Tactics, 65.0, 87.5);
			SetSkill(SkillName.Wrestling, 50.0, 80.0);

			Fame = 9500;
			Karma = -9500;

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
			AddLoot(LootPack.MageryRegs, 10, 20);
		}

		public override bool ShowFameTitle { get { return true; } }
		public override bool AutoDispel => false;
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override int TreasureMapLevel { get { return 2; } }


		public BagusGagakShaman(Serial serial) : base(serial)
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

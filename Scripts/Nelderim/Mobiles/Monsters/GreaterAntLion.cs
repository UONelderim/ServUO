using Server.Items;

namespace Server.Mobiles
{
	[CorpseName("zwloki wielkiego mrowkolwa")]
	public class GreaterAntLion : BaseCreature
	{
		[Constructable]
		public GreaterAntLion() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "wielki mrowkolew";
			Body = 787;
			BaseSoundID = 1006;
			Hue = 0x836;

			SetStr(350, 400);
			SetDex(100, 120);
			SetInt(50, 80);

			SetHits(180, 220);

			SetDamage(10, 24);

			SetDamageType(ResistanceType.Physical, 70);
			SetDamageType(ResistanceType.Poison, 30);

			SetResistance(ResistanceType.Physical, 50, 60);
			SetResistance(ResistanceType.Fire, 35, 45);
			SetResistance(ResistanceType.Cold, 45, 50);
			SetResistance(ResistanceType.Poison, 45, 55);
			SetResistance(ResistanceType.Energy, 40, 45);

			SetSkill(SkillName.MagicResist, 90.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 4500;
			Karma = -4500;

			VirtualArmor = 50;

			PackItem(new FertileDirt(Utility.RandomMinMax(1, 4)));

			switch ( Utility.Random(4) )
			{
				case 0: PackItem(new DullCopperOre(Utility.RandomMinMax(3, 8))); break;
				case 1: PackItem(new ShadowIronOre(Utility.RandomMinMax(3, 8))); break;
				case 2: PackItem(new CopperOre(Utility.RandomMinMax(3, 8))); break;
				case 3: PackItem(new BronzeOre(Utility.RandomMinMax(3, 8))); break;
			}

			// TODO: skeleton
		}


		public override int GetAngerSound()
		{
			return 0x5A;
		}

		public override int GetIdleSound()
		{
			return 0x5A;
		}

		public override int GetAttackSound()
		{
			return 0x164;
		}

		public override int GetHurtSound()
		{
			return 0x187;
		}

		public override int GetDeathSound()
		{
			return 0x1BA;
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average, 2);
		}


		public GreaterAntLion(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);
			writer.Write((int)0);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);
			int version = reader.ReadInt();
		}
	}
}
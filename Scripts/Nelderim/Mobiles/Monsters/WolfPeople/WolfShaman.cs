#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wilczego szamana")]
	public class WolfShaman : BaseCreature
	{
		[Constructable]
		public WolfShaman() : base(AIType.AI_Mage, FightMode.Weakest, 12, 3, 0.2, 0.4)
		{
			Body = 0x190;
			Name = "wilczy szaman";
			Hue = 2940;


			SetStr(81, 205);
			SetDex(191, 215);
			SetInt(126, 150);

			SetHits(250, 300);

			SetDamage(5, 10);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 40);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);

			SetSkill(SkillName.EvalInt, 80.2, 100.0);
			SetSkill(SkillName.Magery, 95.1, 100.0);
			SetSkill(SkillName.Meditation, 27.5, 50.0);
			SetSkill(SkillName.MagicResist, 77.5, 100.0);
			SetSkill(SkillName.Tactics, 65.0, 87.5);
			SetSkill(SkillName.Wrestling, 20.3, 80.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 26;

			LeatherCap Helm = new LeatherCap();
			Helm.Hue = 1187;
			Helm.LootType = LootType.Blessed;
			Helm.ItemID = 5445;
			Helm.Name = "wilcza maska";
			AddItem(Helm);
			Cloak Cloa = new Cloak();
			Cloa.Hue = 1187;
			Cloa.LootType = LootType.Blessed;
			AddItem(Cloa);
			Sandals Boot = new Sandals();
			Boot.Hue = 1187;
			Boot.LootType = LootType.Blessed;
			AddItem(Boot);
			Kilt Pants = new Kilt();
			Pants.Hue = 1187;
			Pants.LootType = LootType.Blessed;
			AddItem(Pants);


			Item hair = new Item(0x203C);
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
			AddLoot(LootPack.Rich);
			AddLoot(LootPack.MageryRegs, 20);
		}

		public override double AttackMasterChance { get { return 0.25; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public WolfShaman(Serial serial) : base(serial)

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

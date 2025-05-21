#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wilczego łucznika")]
	public class WolfArcher : BaseCreature
	{
		[Constructable]
		public WolfArcher() : base(AIType.AI_Archer, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Name = "wilczy łucznik";
			Hue = 2940;
			Body = 0x190;


			SetStr(136, 220);
			SetDex(70, 90);
			SetInt(51, 80);
			SetHits(250, 280);
			SetDamage(16, 20);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 35, 40);
			SetResistance(ResistanceType.Cold, 35, 40);
			SetResistance(ResistanceType.Poison, 25, 30);
			SetResistance(ResistanceType.Energy, 25, 30);


			SetSkill(SkillName.Anatomy, 120.0);
			SetSkill(SkillName.Archery, 70.0, 85.0);
			SetSkill(SkillName.MagicResist, 83.5, 92.5);
			SetSkill(SkillName.Wrestling, 125.0);
			SetSkill(SkillName.Tactics, 120.0);

			Fame = 4000;
			Karma = -4000;


			LeatherCap Helm = new LeatherCap();
			Helm.Hue = 1187;
			Helm.LootType = LootType.Blessed;
			Helm.ItemID = 5445;
			Helm.Name = "wilcza maska";
			AddItem(Helm);
			LeatherChest Chest = new LeatherChest();
			Chest.Hue = 1187;
			Chest.LootType = LootType.Blessed;
			AddItem(Chest);
			LeatherGorget Gorget = new LeatherGorget();
			Gorget.Hue = 1187;
			Gorget.LootType = LootType.Blessed;
			AddItem(Gorget);
			StuddedGloves Gloves = new StuddedGloves();
			Gloves.Hue = 1187;
			Gloves.LootType = LootType.Blessed;
			AddItem(Gloves);
			StuddedArms Arms = new StuddedArms();
			Arms.Hue = 1187;
			Arms.LootType = LootType.Blessed;
			AddItem(Arms);
			StuddedLegs Legs = new StuddedLegs();
			Legs.Hue = 1187;
			Legs.LootType = LootType.Blessed;
			AddItem(Legs);


			Cloak Cloa = new Cloak();
			Cloa.Hue = 1187;
			Cloa.LootType = LootType.Blessed;
			AddItem(Cloa);

			AddItem(new Bow());
			PackItem(new Arrow(Utility.Random(20, 30)));


			Item hair = new Item(0x203C);
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 2);
			AddLoot(LootPack.Meager);
			if (Utility.RandomDouble() < 0.3)
				PackItem(new BowstringCannabis());
		}

		public override double AttackMasterChance { get { return 0.05; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel => false;
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public WolfArcher(Serial serial) : base(serial)
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

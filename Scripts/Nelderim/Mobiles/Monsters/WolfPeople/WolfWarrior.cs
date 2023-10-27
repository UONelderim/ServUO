#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wilczego wojownika")]
	public class WolfWarrior : BaseCreature
	{
		[Constructable]
		public WolfWarrior() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Name = "wilczy wojownik";
			Hue = 2940;
			Body = 0x190;


			SetStr(96, 220);
			SetDex(30, 45);
			SetInt(51, 65);
			SetHits(200, 250);
			SetDamage(14, 16);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 25, 30);
			SetResistance(ResistanceType.Cold, 25, 30);
			SetResistance(ResistanceType.Poison, 10, 20);
			SetResistance(ResistanceType.Energy, 10, 20);


			SetSkill(SkillName.Anatomy, 125.0);
			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 83.5, 92.5);
			SetSkill(SkillName.Swords, 125.0);
			SetSkill(SkillName.Tactics, 125.0);
			SetSkill(SkillName.Lumberjacking, 125.0);

			Fame = 3000;
			Karma = -3000;


			EquipItem(new LeatherCap { Hue = 2707, LootType = LootType.Blessed, ItemID = 5445, Name = "wilcza maska" });
			EquipItem(new StuddedChest { Hue = 2707, LootType = LootType.Blessed });
			EquipItem(new StuddedGorget { Hue = 2707, LootType = LootType.Blessed });
			EquipItem(new BoneGloves { Hue = 2707, LootType = LootType.Blessed });
			EquipItem(new BoneArms { Hue = 2707, LootType = LootType.Blessed });
			EquipItem(new BoneLegs { Hue = 2707, LootType = LootType.Blessed });
			EquipItem(new Katana { Hue = 424, Movable = false, LootType = LootType.Blessed });
			EquipItem(new MetalShield { Hue = 424, Movable = false, LootType = LootType.Blessed });
			EquipItem(new Cloak { Hue = 2707, LootType = LootType.Blessed });
			
			HairItemID = 0x203C;
			HairHue = Race.RandomHairHue();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Rich, 2);
			AddLoot(LootPack.Meager);
		}

		public override double AttackMasterChance { get { return 0.05; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel { get { return true; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }

		public WolfWarrior(Serial serial) : base(serial)
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

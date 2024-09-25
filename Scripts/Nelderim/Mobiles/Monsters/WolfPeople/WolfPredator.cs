#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wilczego predatora")]
	public class WolfPredator : BaseCreature
	{
		[Constructable]
		public WolfPredator() : base(AIType.AI_NecroMage, FightMode.Weakest, 12, 3, 0.2, 0.4)
		{
			Body = 0x190;
			Name = "wilczy predator";
			Hue = 2940;


			SetStr(250, 320);
			SetDex(160, 200);
			SetInt(150, 200);

			SetHits(300, 350);

			SetDamage(5, 10);

			SetDamageType(ResistanceType.Physical, 40);
			SetDamageType(ResistanceType.Poison, 30);
			SetDamageType(ResistanceType.Energy, 30);

			SetResistance(ResistanceType.Physical, 45, 50);
			SetResistance(ResistanceType.Fire, 40, 45);
			SetResistance(ResistanceType.Cold, 40, 45);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 40, 45);

			SetSkill(SkillName.Necromancy, 85.0, 100.0);
			SetSkill(SkillName.SpiritSpeak, 95.0, 100.0);
			SetSkill(SkillName.Meditation, 70.5, 90.0);
			SetSkill(SkillName.MagicResist, 90.5, 100.0);
			SetSkill(SkillName.Tactics, 80.0, 100.0);
			SetSkill(SkillName.Wrestling, 60.0, 80.0);
			SetSkill(SkillName.Anatomy, 90.1, 100.0);
			SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill(SkillName.Swords, 90.1, 100.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 26;

			BoneHelm Helm = new BoneHelm();
			Helm.Hue = 1187;
			Helm.LootType = LootType.Blessed;
			Helm.ItemID = 5445;
			AddItem(Helm);

			BoneChest Chest = new BoneChest();
			Chest.Hue = 1187;
			Chest.LootType = LootType.Blessed;
			AddItem(Chest);

			BoneLegs Legs = new BoneLegs();
			Legs.Hue = 1187;
			Legs.LootType = LootType.Blessed;
			AddItem(Legs);

			BoneArms Arms = new BoneArms();
			Arms.Hue = 1187;
			Arms.LootType = LootType.Blessed;
			AddItem(Arms);

			BoneGloves Gloves = new BoneGloves();
			Gloves.Hue = 1187;
			Gloves.LootType = LootType.Blessed;
			AddItem(Gloves);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 1187;
			Cloa.LootType = LootType.Blessed;
			AddItem(Cloa);

			Sandals Boot = new Sandals();
			Boot.Hue = 1187;
			Boot.LootType = LootType.Blessed;
			AddItem(Boot);

			Daisho sword = new Daisho();
			sword.Movable = false;
			EquipItem(sword);


			Item hair = new Item(0x203C);
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			SetWeaponAbility(WeaponAbility.DoubleStrike);
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

		public WolfPredator(Serial serial) : base(serial)

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

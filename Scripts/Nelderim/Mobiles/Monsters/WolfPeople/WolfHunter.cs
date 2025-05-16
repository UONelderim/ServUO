#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki wilczego lowcy")]
	public class WolfHunter : BaseCreature
	{
		[Constructable]
		public WolfHunter() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Body = 0x190;
			Name = "wilczy lowca";
			Hue = 2940;


			SetStr(351, 400);
			SetDex(50, 70);
			SetInt(151, 200);

			SetHits(341, 400);

			SetDamage(15, 18);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 45, 50);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 20, 25);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.Anatomy, 90.1, 100.0);
			SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Fencing, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 95.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);
			SetSkill(SkillName.Parry, 90.1, 100.0);

			Fame = 5000;
			Karma = -5000;


			LeatherCap Helm = new LeatherCap();
			Helm.Hue = 1187;
			Helm.LootType = LootType.Blessed;
			Helm.ItemID = 5445;
			Helm.Name = "wilcza maska";
			AddItem(Helm);

			StuddedChest Chest = new StuddedChest();
			Chest.Hue = 1187;
			Chest.LootType = LootType.Blessed;
			AddItem(Chest);

			StuddedGorget Gorget = new StuddedGorget();
			Gorget.Hue = 1187;
			Gorget.LootType = LootType.Blessed;
			AddItem(Gorget);

			BoneLegs Legs = new BoneLegs();
			Legs.Hue = 1187;
			Legs.LootType = LootType.Blessed;
			AddItem(Legs);

			BoneArms Arms = new BoneArms();
			Arms.Hue = 1187;
			Arms.LootType = LootType.Blessed;
			AddItem(Arms);

			Cloak Cloak = new Cloak();
			Cloak.Hue = 1187;
			Cloak.LootType = LootType.Blessed;
			AddItem(Cloak);

			Kilt Pants = new Kilt();
			Pants.Hue = 1187;
			Pants.LootType = LootType.Blessed;
			AddItem(Pants);

			Tekagi Szpony = new Tekagi();
			Szpony.Name = "Szpony";
			AddItem(Szpony);

			new FrenziedOstard().Rider = this;
			Hue = 33885;

			Item hair = new Item(0x203C);
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			VirtualArmor = 48;

			Container pack = new Backpack();

			pack.DropItem(new Bandage(Utility.RandomMinMax(5, 10)));
			pack.DropItem(new Bandage(Utility.RandomMinMax(5, 10)));
			pack.DropItem(Loot.RandomGem());

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if (mount != null)
				mount.Rider = null;

			if (mount is Mobile)
				((Mobile)mount).Kill();

			return base.OnBeforeDeath();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich);
			AddLoot(LootPack.Meager, 2);
		}

		public override double AttackMasterChance { get { return 0.15; } }
		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel => false;
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }


		public WolfHunter(Serial serial) : base(serial)
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

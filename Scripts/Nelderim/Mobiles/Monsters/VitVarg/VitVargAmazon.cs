#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class VitVargAmazon : BaseCreature
	{
		[Constructable]
		public VitVargAmazon() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.3)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Varg Amazonka";
			Hue = Race.RandomSkinHue();
			Body = 0x191;
			Female = true;
			Name = NameList.RandomName("female");


			SetStr(140, 180);
			SetDex(80, 120);
			SetInt(40, 60);

			SetDamage(8, 10);


			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 55, 65);
			SetResistance(ResistanceType.Poison, 45, 55);
			SetResistance(ResistanceType.Energy, 45, 55);


			SetSkill(SkillName.Anatomy, 125.0);
			SetSkill(SkillName.Fencing, 100.0, 120.0);
			SetSkill(SkillName.MagicResist, 83.5, 92.5);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Ninjitsu, 100.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 35;

			LeatherNinjaHood Helm = new LeatherNinjaHood();
			Helm.Hue = 856;
			Helm.Movable = false;
			EquipItem(Helm);

			FemaleLeatherChest chest = new FemaleLeatherChest();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem(chest);

			LeatherSkirt legs = new LeatherSkirt();
			legs.Hue = 856;
			legs.Movable = false;
			EquipItem(legs);

			LeatherNinjaMitts Gloves = new LeatherNinjaMitts();
			Gloves.Hue = 2106;
			Gloves.Movable = false;
			EquipItem(Gloves);

			Boots Boot = new Boots();
			Boot.Hue = 1109;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem(sash);

			Tekagi sword = new Tekagi();
			sword.Hue = 781;
			sword.Movable = false;
			EquipItem(sword);

			SetWeaponAbility(WeaponAbility.TalonStrike);
			SetWeaponAbility(WeaponAbility.DualWield);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public VitVargAmazon(Serial serial) : base(serial)
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

#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class VitVargCutler : BaseCreature
	{
		[Constructable]
		public VitVargCutler() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.15, 0.3)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Varg Nozownik";
			Hue = Race.RandomSkinHue();

			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
				AddItem(new Kilt(Utility.RandomNeutralHue()));
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
				AddItem(new Kilt(Utility.RandomNeutralHue()));
			}

			SetStr(140, 160);
			SetDex(100, 140);
			SetInt(40, 60);

			SetDamage(12, 14);


			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 30, 40);
			SetResistance(ResistanceType.Cold, 50, 55);
			SetResistance(ResistanceType.Poison, 40, 45);
			SetResistance(ResistanceType.Energy, 40, 45);


			SetSkill(SkillName.Anatomy, 125.0);
			SetSkill(SkillName.Fencing, 120.0, 140.0);
			SetSkill(SkillName.Poisoning, 90.0, 100.0);
			SetSkill(SkillName.MagicResist, 83.5, 92.5);
			SetSkill(SkillName.Swords, 120.0);
			SetSkill(SkillName.Tactics, 100.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			NorseHelm Helm = new NorseHelm();
			Helm.Hue = 856;
			Helm.Movable = false;
			EquipItem(Helm);

			Shirt chest = new Shirt();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem(chest);

			LeatherGloves Gloves = new LeatherGloves();
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

			Kama sword = new Kama();
			sword.Hue = 781;
			sword.Movable = false;
			EquipItem(sword);

			SetWeaponAbility(WeaponAbility.Dismount);
		}

		public override double WeaponAbilityChance => 0.3;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }

		public override Poison HitPoison { get { return Poison.Greater; } }
		public override double HitPoisonChance { get { return 0.4; } }


		public VitVargCutler(Serial serial) : base(serial)
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

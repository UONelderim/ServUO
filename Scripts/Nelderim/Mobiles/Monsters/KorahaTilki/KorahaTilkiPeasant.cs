#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class KorahaTilkiPeasant : BaseCreature
	{
		[Constructable]
		public KorahaTilkiPeasant() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- Tilki Wiesniak";
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

			SetStr(120, 140);
			SetDex(50, 80);
			SetInt(40, 60);

			SetHits(120, 160);

			SetDamage(6, 10);


			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 25, 35);
			SetResistance(ResistanceType.Fire, 45, 55);
			SetResistance(ResistanceType.Cold, 30, 40);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);


			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Fencing, 50.0, 70.0);
			SetSkill(SkillName.Macing, 50.0, 70.0);
			SetSkill(SkillName.MagicResist, 95.0, 100.0);
			SetSkill(SkillName.Swords, 90.0, 100.0);
			SetSkill(SkillName.Tactics, 120.0);

			Fame = 2000;
			Karma = -2000;

			VirtualArmor = 45;

			Bandana Helm = new Bandana();
			Helm.Hue = 1319;
			Helm.Movable = false;
			EquipItem(Helm);

			Tunic chest = new Tunic();
			chest.Hue = 1319;
			chest.Movable = false;
			EquipItem(chest);

			ShortPants legs = new ShortPants();
			legs.Hue = 1319;
			legs.Movable = false;
			EquipItem(legs);

			Sandals Boot = new Sandals();
			Boot.Hue = 48;
			AddItem(Boot);

			BodySash sash = new BodySash();
			sash.Hue = 48;
			sash.Movable = false;
			EquipItem(sash);

			Scythe sword = new Scythe();
			sword.Hue = 1437;
			sword.Movable = false;
			EquipItem(sword);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Meager);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool ShowFameTitle { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }


		public KorahaTilkiPeasant(Serial serial) : base(serial)
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

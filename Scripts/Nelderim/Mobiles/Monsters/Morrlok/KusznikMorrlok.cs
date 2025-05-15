#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class KusznikMorrlok : BaseCreature
	{
		[Constructable]
		public KusznikMorrlok() : base(AIType.AI_Archer, FightMode.Closest, 12, 4, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();

			Title = "- kusznik";
			Hue = Race.RandomSkinHue();

			if (this.Female = Utility.RandomBool())
			{
				Body = 0x191;
				Name = NameList.RandomName("female");
				AddItem(new Skirt(Utility.RandomNeutralHue()));
			}
			else
			{
				Body = 0x190;
				Name = NameList.RandomName("male");
				AddItem(new ShortPants(Utility.RandomNeutralHue()));
			}


			SetStr(126, 145);
			SetDex(96, 115);
			SetInt(51, 65);

			SetDamage(17, 29);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 35, 45);
			SetResistance(ResistanceType.Cold, 35, 45);
			SetResistance(ResistanceType.Poison, 30, 40);
			SetResistance(ResistanceType.Energy, 30, 40);


			SetSkill(SkillName.Archery, 80.0, 100.5);
			SetSkill(SkillName.Poisoning, 60.0, 82.5);
			SetSkill(SkillName.MagicResist, 57.5, 80.0);
			SetSkill(SkillName.Swords, 60.0, 82.5);
			SetSkill(SkillName.Tactics, 60.0, 82.5);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			Item hair = new Item(Utility.RandomList(0x203C));
			hair.Hue = Race.RandomHairHue();
			hair.Layer = Layer.Hair;
			hair.Movable = false;
			AddItem(hair);

			ChainChest chest = new ChainChest();
			chest.Hue = 2106;
			chest.Movable = false;
			EquipItem(chest);

			StuddedGloves Gloves = new StuddedGloves();
			Gloves.Hue = 2106;
			Gloves.Movable = false;
			EquipItem(Gloves);

			RingmailLegs legs = new RingmailLegs();
			legs.Hue = 2106;
			legs.Movable = false;
			EquipItem(legs);

			Boots Boot = new Boots();
			Boot.Hue = 1109;
			AddItem(Boot);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 1109;
			AddItem(Cloa);

			Bandana band = new Bandana();
			band.Hue = 1142;
			AddItem(band);

			BodySash sash = new BodySash();
			sash.Hue = 1142;
			AddItem(sash);

			AddItem(new Crossbow());
			PackItem(new Bolt(Utility.Random(25, 30)));

			SetWeaponAbility(WeaponAbility.ConcussionBlow);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Meager);
		}

		public override bool ShowFameTitle { get { return true; } }
		public override bool AutoDispel => false;
		public override bool AlwaysMurderer { get { return true; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Regular; } }


		public KusznikMorrlok(Serial serial) : base(serial)
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

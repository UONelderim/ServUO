#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class PatrolTasandorski : BaseCreature
	{
		[Constructable]
		public PatrolTasandorski() : base(AIType.AI_Melee, FightMode.Criminal, 12, 1, 0.2, 0.4)
		{
			SpeechHue = Utility.RandomDyedHue();
			Title = "- Patrol Tasandorski";
			Hue = Race.RandomSkinHue();

			if (Utility.RandomBool())
			{
				Female = true;
				Body = 0x191;
				Name = NameList.RandomName("female");
			}
			else
			{
				Female = false;
				Body = 0x190;
				Name = NameList.RandomName("male");
			}

			SetStr(160, 200);
			SetDex(100, 120);
			SetInt(50, 65);

			SetHits(200, 240);

			SetDamage(14, 18);


			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Fire, 40);

			SetResistance(ResistanceType.Physical, 35, 45);
			SetResistance(ResistanceType.Fire, 55, 65);
			SetResistance(ResistanceType.Cold, 40, 45);
			SetResistance(ResistanceType.Poison, 35, 50);
			SetResistance(ResistanceType.Energy, 35, 50);
			
			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.Swords, 50.0, 70.0);
			SetSkill(SkillName.MagicResist, 80.0, 90.0);
			SetSkill(SkillName.Fencing, 90.0, 110.0);
			SetSkill(SkillName.Tactics, 100.0);
			
			SetWeaponAbility(WeaponAbility.ArmorIgnore);
		}

		protected override void OnCreate()
		{
			base.OnCreate();
			
			EquipItem(new NorseHelm { Movable = false });
			EquipItem(new PlateChest { Movable = false });
			EquipItem(new PlateGorget { Movable = false });
			EquipItem(new PlateGloves { Movable = false });
			EquipItem(new PlateLegs { Movable = false });
			EquipItem(new PlateArms { Movable = false });
			EquipItem(new Boots { Movable = false, Hue = 2894 });
			EquipItem(new Cloak { Movable = false, Hue = 2894 });
			EquipItem(new BodySash { Movable = false, Hue = 2894 });
			EquipItem(new Spear { Movable = false });
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.Average);
		}


		public override bool ShowFameTitle => true;
		public override bool CanRummageCorpses => true;
		public override Poison PoisonImmune => Poison.Regular;
		public override bool DeleteCorpseOnDeath => true;


		public PatrolTasandorski(Serial serial) : base(serial)
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

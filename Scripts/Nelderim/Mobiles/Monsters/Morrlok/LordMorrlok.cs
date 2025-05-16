#region References

using System;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class LordMorrlok : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.15; } }

		[Constructable]
		public LordMorrlok() : base(AIType.AI_Melee, FightMode.Strongest, 12, 1, 0.2, 0.4)
		{
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


			SetStr(351, 400);
			SetDex(181, 230);
			SetInt(151, 200);

			SetHits(341, 400);

			SetDamage(15, 20);

			SetDamageType(ResistanceType.Physical, 100);

			SetResistance(ResistanceType.Physical, 40, 50);
			SetResistance(ResistanceType.Fire, 45, 50);
			SetResistance(ResistanceType.Cold, 40, 50);
			SetResistance(ResistanceType.Poison, 20, 25);
			SetResistance(ResistanceType.Energy, 40, 50);

			SetSkill(SkillName.Anatomy, 90.1, 100.0);
			SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Swords, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 95.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);
			SetSkill(SkillName.Parry, 90.1, 100.0);

			Fame = 15000;
			Karma = -15000;

			PlateHelm Helm = new PlateHelm();
			Helm.Hue = 1157;
			Helm.Movable = false;
			EquipItem(Helm);

			PlateChest chest = new PlateChest();
			chest.Hue = 1157;
			chest.Movable = false;
			EquipItem(chest);

			PlateGorget Gorget = new PlateGorget();
			Gorget.Hue = 1157;
			Gorget.Movable = false;
			EquipItem(Gorget);

			PlateGloves Gloves = new PlateGloves();
			Gloves.Hue = 1157;
			Gloves.Movable = false;
			EquipItem(Gloves);

			PlateLegs legs = new PlateLegs();
			legs.Hue = 1157;
			legs.Movable = false;
			EquipItem(legs);

			PlateArms arms = new PlateArms();
			arms.Hue = 1157;
			arms.Movable = false;
			EquipItem(arms);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 32;
			EquipItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 32;
			EquipItem(sash);

			DoubleAxe axe = new DoubleAxe();
			axe.Hue = 2406;
			axe.Movable = false;
			EquipItem(axe);

			MorrlokWarHorse mount = new MorrlokWarHorse();

			mount.ControlMaster = this;
			mount.Controlled = true;
			mount.InvalidateProperties();

			mount.Rider = this;

			VirtualArmor = 48;

			SetWeaponAbility(WeaponAbility.WhirlwindAttack);
		}

		public override bool OnBeforeDeath()
		{
			IMount mount = this.Mount;

			if ( Mount != null )
				Mount.Rider = null;

			return base.OnBeforeDeath();
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
			AddLoot(LootPack.Gems, 2);
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (from != null && !willKill && amount > 5 && from.Player && 5 > Utility.Random(300))
			{
				string[] toSay =
				{
					"{0}!! Zginiesz glupcze!", "{0}!! Juz widze jak twoje cialo tocza robale!",
					"{0}!! Jesli masz choc troche rozumu to lepiej wiej zanim zatopie w tobie moj topor!",
				};

				this.Say(true, String.Format(toSay[Utility.Random(toSay.Length)], from.Name));
			}

			base.OnDamage(amount, from, willKill);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel => false;
		public override bool ShowFameTitle { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }


		public LordMorrlok(Serial serial) : base(serial)
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

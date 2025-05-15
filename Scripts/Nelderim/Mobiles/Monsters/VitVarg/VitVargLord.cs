#region References

using System;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	public class VitVargLord : BaseCreature
	{
		public override double AttackMasterChance { get { return 0.2; } }

		[Constructable]
		public VitVargLord() : base(AIType.AI_Melee, FightMode.Strongest, 12, 1, 0.2, 0.4)
		{
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


			SetStr(350, 400);
			SetDex(140, 180);
			SetInt(100, 120);

			SetHits(400, 500);

			SetDamage(22, 26);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Cold, 40);

			SetResistance(ResistanceType.Physical, 45, 50);
			SetResistance(ResistanceType.Fire, 30, 45);
			SetResistance(ResistanceType.Cold, 60, 65);
			SetResistance(ResistanceType.Poison, 45, 55);
			SetResistance(ResistanceType.Energy, 45, 55);

			SetSkill(SkillName.Anatomy, 100.0, 120.0);
			SetSkill(SkillName.Healing, 80.1, 100.0);
			SetSkill(SkillName.MagicResist, 120.1, 130.0);
			SetSkill(SkillName.Swords, 90.1, 100.0);
			SetSkill(SkillName.Tactics, 95.1, 100.0);
			SetSkill(SkillName.Wrestling, 90.1, 100.0);
			SetSkill(SkillName.Parry, 90.1, 100.0);

			Fame = 12000;
			Karma = -12000;

			PlateHelm Helm = new PlateHelm();
			Helm.Hue = 856;
			Helm.Movable = false;
			EquipItem(Helm);

			PlateChest chest = new PlateChest();
			chest.Hue = 856;
			chest.Movable = false;
			EquipItem(chest);

			PlateGorget Gorget = new PlateGorget();
			Gorget.Hue = 856;
			Gorget.Movable = false;
			EquipItem(Gorget);

			PlateGloves Gloves = new PlateGloves();
			Gloves.Hue = 856;
			Gloves.Movable = false;
			EquipItem(Gloves);

			PlateLegs legs = new PlateLegs();
			legs.Hue = 856;
			legs.Movable = false;
			EquipItem(legs);

			PlateArms arms = new PlateArms();
			arms.Hue = 856;
			arms.Movable = false;
			EquipItem(arms);

			Cloak Cloa = new Cloak();
			Cloa.Hue = 288;
			Cloa.Movable = false;
			AddItem(Cloa);

			BodySash sash = new BodySash();
			sash.Hue = 288;
			sash.Movable = false;
			EquipItem(sash);

			Bardiche axe = new Bardiche();
			axe.Hue = 781;
			axe.Movable = false;
			EquipItem(axe);

			VirtualArmor = 50;

			Container pack = new Backpack();

			SetWeaponAbility(WeaponAbility.ArmorIgnore);
		}

		public override double WeaponAbilityChance => 0.3;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
			AddLoot(LootPack.Gems, 1);
		}

		public override void OnDamage(int amount, Mobile from, bool willKill)
		{
			if (from != null && !willKill && amount > 5 && from.Player && 5 > Utility.Random(300))
			{
				string[] toSay =
				{
					"{0}!! Nie wyjdziesz stad glupcze!", "{0}!! Gin scierwo!", "{0}!! Niech zyje Bialy Wilk!",
				};

				this.Say(true, String.Format(toSay[Utility.Random(toSay.Length)], from.Name));
			}

			base.OnDamage(amount, from, willKill);
		}

		public override bool AlwaysMurderer { get { return true; } }
		public override bool AutoDispel => false;
		public override bool ShowFameTitle { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }


		public VitVargLord(Serial serial) : base(serial)
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

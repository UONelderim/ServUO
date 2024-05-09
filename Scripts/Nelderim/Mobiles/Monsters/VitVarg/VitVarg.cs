#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki bialego wilka")]
	public class VitVarg : BaseCreature
	{
		public override bool IgnoreYoungProtection => true;

		[Constructable]
		public VitVarg() : base(AIType.AI_Mage, FightMode.Closest, 14, 1, 0.2, 0.4)
		{
			Name = "Bialy Wilk";
			Body = 311;
			Hue = 2461;

			SetStr(1800, 2000);
			SetDex(300);
			SetInt(600);

			SetHits(4000);

			SetDamage(24, 32);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Cold, 50);

			SetResistance(ResistanceType.Physical, 80);
			SetResistance(ResistanceType.Fire, 60);
			SetResistance(ResistanceType.Cold, 70);
			SetResistance(ResistanceType.Poison, 60);
			SetResistance(ResistanceType.Energy, 60);

			SetSkill(SkillName.DetectHidden, 80.0);
			SetSkill(SkillName.EvalInt, 100.0);
			SetSkill(SkillName.Magery, 100.0);
			SetSkill(SkillName.Meditation, 100.0);
			SetSkill(SkillName.MagicResist, 120.0);
			SetSkill(SkillName.Tactics, 120.0);
			SetSkill(SkillName.Wrestling, 120.0);
			SetSkill(SkillName.Swords, 120.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 55;

			SetWeaponAbility(WeaponAbility.BleedAttack);
			SetWeaponAbility(WeaponAbility.CrushingBlow);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.UltraRich, 2);
		}

		public override int GetIdleSound()
		{
			return 0x2CE;
		}

		public override int GetDeathSound()
		{
			return 0x2C1;
		}

		public override int GetHurtSound()
		{
			return 0x2D1;
		}

		public override int GetAttackSound()
		{
			return 0x2C8;
		}


		public override bool BardImmune => false;
		public override Poison PoisonImmune => Poison.Lethal;
		public override double AttackMasterChance => 0.10;
		public override double SwitchTargetChance => 0.10;
		public override double DispelDifficulty => 120.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => true;
		public override bool CanRummageCorpses => true;
		public override bool AllureImmune => true;

		public override int TreasureMapLevel => 1;

		public VitVarg(Serial serial) : base(serial)
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

			if (BaseSoundID == 357)
				BaseSoundID = -1;
		}
	}
}

#region References

using Nelderim;
using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki pustynnej pluskwy")]
	public class TilkiBug : BaseCreature
	{
		public override bool IgnoreYoungProtection => true;

		[Constructable]
		public TilkiBug() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "Wielka Pluskwa";
			Body = 315;
			Hue = 1281;

			SetStr(1200, 1300);
			SetDex(500, 600);
			SetInt(300);

			SetHits(4500);

			SetDamage(30, 35);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Poison, 20);
			SetDamageType(ResistanceType.Energy, 20);

			SetResistance(ResistanceType.Physical, 70);
			SetResistance(ResistanceType.Fire, 50);
			SetResistance(ResistanceType.Cold, 60);
			SetResistance(ResistanceType.Poison, 60);
			SetResistance(ResistanceType.Energy, 60);

			SetSkill(SkillName.DetectHidden, 80.0);
			SetSkill(SkillName.MagicResist, 155.1, 160.0);
			SetSkill(SkillName.Meditation, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 120.0, 160.0);

			Fame = 15000;
			Karma = -15000;

			VirtualArmor = 50;

			SetWeaponAbility(WeaponAbility.Dismount);
			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
		}

		public override void GenerateLoot()
		{
			AddLoot(NelderimLoot.RangerScrolls);
		}

		public override bool BardImmune => true;
		public override Poison PoisonImmune => Poison.Lethal;
		public override double AttackMasterChance => 0.10;
		public override double SwitchTargetChance => 0.10;
		public override double DispelDifficulty => 120.0;
		public override double DispelFocus => 45.0;
		public override bool AutoDispel => false;
		public override bool CanRummageCorpses => true;
		public override bool Unprovokable => true;
		public override bool Uncalmable => true;

		public override int TreasureMapLevel => 1;

		public override int GetAttackSound() => 0x34C;

		public override int GetHurtSound() => 0x354;

		public override int GetAngerSound() => 0x34C;

		public override int GetIdleSound() => 0x34C;

		public override int GetDeathSound() => 0x354;

		public TilkiBug(Serial serial) : base(serial)
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

			if (BaseSoundID == 660)
				BaseSoundID = -1;
		}
	}
}

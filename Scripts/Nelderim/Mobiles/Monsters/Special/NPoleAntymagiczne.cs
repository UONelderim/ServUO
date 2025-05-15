#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zgliszcza pola antymagicznego")]
	public class NPoleAntymagiczne : BaseCreature
	{
		public override bool BardImmune { get { return true; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override double DispelDifficulty { get { return 135.0; } }
		public override double DispelFocus { get { return 45.0; } }
		public override bool AutoDispel => false;
		public override bool CanRummageCorpses { get { return true; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool IgnoreHonor { get { return true; } }

		[Constructable]
		public NPoleAntymagiczne() : base(AIType.AI_Melee, FightMode.Weakest, 12, 1, 0.1, 0.2)
		{
			Name = "pole antymagiczne ";
			Body = 58;
			Hue = 1393;


			SetStr(1286, 1385);
			SetDex(310, 365);
			SetInt(1800, 1900);

			SetHits(38000);

			SetDamage(26, 35);

			SetDamageType(ResistanceType.Fire, 25);
			SetDamageType(ResistanceType.Energy, 75);

			SetResistance(ResistanceType.Physical, 100);
			SetResistance(ResistanceType.Fire, 100);
			SetResistance(ResistanceType.Cold, 100);
			SetResistance(ResistanceType.Poison, 100);
			SetResistance(ResistanceType.Energy, 100);

			SetSkill(SkillName.Anatomy, 100.0);
			SetSkill(SkillName.MagicResist, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0);

			Fame = 34000;
			Karma = -34000;

			VirtualArmor = 90;
			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.BleedAttack);
		}

		public NPoleAntymagiczne(Serial serial) : base(serial)
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

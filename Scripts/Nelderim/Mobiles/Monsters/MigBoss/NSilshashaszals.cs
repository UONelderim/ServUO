#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki silshashaszalsa")]
	public class NSilshashaszals : BaseCreature
	{
		public override bool BardImmune { get { return false; } }
		public override int TreasureMapLevel { get { return 5; } }
		public override double AttackMasterChance { get { return 0.15; } }
		public override double SwitchTargetChance { get { return 0.15; } }
		public override Poison PoisonImmune { get { return Poison.Greater; } }
		public override Poison HitPoison { get { return Poison.Lesser; } }

		[Constructable]
		public NSilshashaszals() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.25, 0.5)
		{
			Body = 36;
			Hue = 2150;
			Name = "silshashaszals - krol jaszczuroludzi";

			BaseSoundID = 417;

			SetStr(250, 300);
			SetDex(280, 300);
			SetInt(300, 320);
			SetHits(3000, 5000);

			SetDamage(18, 22);

			SetDamageType(ResistanceType.Physical, 50);
			SetDamageType(ResistanceType.Energy, 50);

			SetResistance(ResistanceType.Physical, 45, 55);
			SetResistance(ResistanceType.Fire, 40, 50);
			SetResistance(ResistanceType.Cold, 25, 35);
			SetResistance(ResistanceType.Poison, 25, 35);
			SetResistance(ResistanceType.Energy, 25, 35);

			SetSkill(SkillName.MagicResist, 120.7, 140.0);
			SetSkill(SkillName.Tactics, 80.0, 90.0);
			SetSkill(SkillName.Wrestling, 80.0, 90.0);
			SetSkill(SkillName.EvalInt, 80.0, 90.0);
			SetSkill(SkillName.Magery, 80.0, 90.0);

			Fame = 22500;
			Karma = -22500;

			VirtualArmor = 80;

			AddItem(new LightSource());

			SetWeaponAbility(WeaponAbility.MortalStrike);
		}

		public override void GenerateLoot()
		{
			AddLoot(LootPack.SuperBoss);
		}

		public NSilshashaszals(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}

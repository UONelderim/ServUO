#region References

using Server.Items;

#endregion

namespace Server.Mobiles
{
	[CorpseName("zwloki pomniejszej pustynnej pluskwy")]
	public class TilkiSmallBug : BaseCreature
	{
		public override bool IgnoreYoungProtection => true;

		[Constructable]
		public TilkiSmallBug() : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.2, 0.4)
		{
			Name = "Pomniejsza Pluskwa";
			Body = 315;
			Hue = 1510;

			SetStr(1000);
			SetDex(500);
			SetInt(200);

			SetHits(2000);

			SetDamage(20, 30);

			SetDamageType(ResistanceType.Physical, 60);
			SetDamageType(ResistanceType.Poison, 20);
			SetDamageType(ResistanceType.Energy, 20);

			SetResistance(ResistanceType.Physical, 70);
			SetResistance(ResistanceType.Fire, 50);
			SetResistance(ResistanceType.Cold, 55);
			SetResistance(ResistanceType.Poison, 80);
			SetResistance(ResistanceType.Energy, 70);

			SetSkill(SkillName.MagicResist, 120.0);
			SetSkill(SkillName.Meditation, 100.0);
			SetSkill(SkillName.Tactics, 100.0);
			SetSkill(SkillName.Wrestling, 100.0, 120.0);

			Fame = 5000;
			Karma = -5000;

			VirtualArmor = 40;

			PackItem(new PosokaPluskwy());

			SetWeaponAbility(WeaponAbility.ParalyzingBlow);
		}

		public override double WeaponAbilityChance => 0.2;

		public override void GenerateLoot()
		{
			AddLoot(LootPack.FilthyRich, 1);
		}

		public override bool BardImmune { get { return false; } }
		public override Poison PoisonImmune { get { return Poison.Lethal; } }
		public override bool AutoDispel { get { return true; } }
		public override int TreasureMapLevel { get { return 6; } }
		public override bool CanRummageCorpses { get { return true; } }
		public override bool Unprovokable { get { return true; } }
		public override bool Uncalmable { get { return true; } }


		public override int GetAttackSound()
		{
			return 0x34C;
		}

		public override int GetHurtSound()
		{
			return 0x354;
		}

		public override int GetAngerSound()
		{
			return 0x34C;
		}

		public override int GetIdleSound()
		{
			return 0x34C;
		}

		public override int GetDeathSound()
		{
			return 0x354;
		}

		public TilkiSmallBug(Serial serial) : base(serial)
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

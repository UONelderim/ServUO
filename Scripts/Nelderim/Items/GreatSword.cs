namespace Server.Items
{
	[FlipableAttribute(0x26CE, 0x26CF)]
	public class GreatSword : BaseSword
	{
		public override WeaponAbility PrimaryAbility { get { return WeaponAbility.WhirlwindAttack; } }
		public override WeaponAbility SecondaryAbility { get { return WeaponAbility.Disarm; } }

		public override int StrengthReq { get { return 50; } }
		public override int MinDamage { get { return 20; } }
		public override int MaxDamage { get { return 24; } }
		public override float Speed { get { return 5.0f; } }
		public override int DefHitSound { get { return 0x237; } }
		public override int DefMissSound { get { return 0x23A; } }

		public override int InitMinHits { get { return 31; } }
		public override int InitMaxHits { get { return 71; } }

		[Constructable]
		public GreatSword() : base(0x26CE)
		{
			Weight = 10.0;
		}

		public GreatSword(Serial serial) : base(serial)
		{
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write((int)0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}
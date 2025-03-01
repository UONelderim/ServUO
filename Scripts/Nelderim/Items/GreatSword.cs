namespace Server.Items
{
	[FlipableAttribute(0x26CE, 0x26CF)]
	public class GreatSword : BaseSword
	{
		public override WeaponAbility PrimaryAbility => WeaponAbility.WhirlwindAttack;
		public override WeaponAbility SecondaryAbility => WeaponAbility.Disarm;

		public override int StrengthReq => 50;
		public override int MinDamage => 20;
		public override int MaxDamage => 24;
		public override float Speed => 5.0f;
		public override int DefHitSound => 0x237;
		public override int DefMissSound => 0x23A;

		public override int InitMinHits => 31;
		public override int InitMaxHits => 71;

		[CommandProperty(AccessLevel.GameMaster)]
		public override Layer Layer => Layer.TwoHanded;

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

			writer.Write(0); // version
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();
		}
	}
}

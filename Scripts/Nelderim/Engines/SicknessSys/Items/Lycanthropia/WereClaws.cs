#region References

using Server.Items;
using Server.Mobiles;

#endregion

namespace Server.SicknessSys.Items
{
	[Flipable(0x27Ab, 0x27F6)]
	public class WereClaws : BaseSword
	{
		public PlayerMobile pm { get; set; }

		public override string DefaultName => "Were Claws";

		[Constructable]
		public WereClaws(PlayerMobile player) : base(0x27AB)
		{
			pm = player;
			Hue = 1177;
			Weight = 5.0;
			Layer = Layer.TwoHanded;
			LootType = LootType.Blessed;
		}

		public WereClaws(Serial serial) : base(serial)
		{
		}

		public override bool OnEquip(Mobile from)
		{
			if (from != pm || pm == null)
				return false;

			return base.OnEquip(from);
		}

		public override WeaponAbility PrimaryAbility => WeaponAbility.DualWield;

		public override WeaponAbility SecondaryAbility => WeaponAbility.TalonStrike;

		//public override int StrengthReq => 10;
		//public override int MinDamage => 10;
		//public override int MaxDamage => 13;
		//public override float Speed => 2.00f;
		public override int DefHitSound => 0x238;
		public override int DefMissSound => 0x232;
		public override int InitMinHits => 35;
		public override int InitMaxHits => 60;

		public override SkillName DefSkill

		{
			get
			{
				return SkillName.Tactics;
			}
		}

		public override WeaponType DefType
		{
			get
			{
				return WeaponType.Piercing;
			}
		}

		public override WeaponAnimation DefAnimation
		{
			get
			{
				return WeaponAnimation.Pierce1H;
			}
		}

		public override void Serialize(GenericWriter writer)
		{
			base.Serialize(writer);

			writer.Write(0); // version

			writer.Write(pm);
		}

		public override void Deserialize(GenericReader reader)
		{
			base.Deserialize(reader);

			int version = reader.ReadInt();

			pm = reader.ReadMobile() as PlayerMobile;
		}
	}
}

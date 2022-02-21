//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//

#region References

using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class ExplosiveLesserTrap : BaseTinkerTrap
	{
		private static readonly string m_ArmedName = "uzbrojona pomniejsza wybuchowa pułapka";
		private static readonly string m_UnarmedName = "nieuzbrojona pomniejsza wybuchowa pułapka";
		private static readonly double m_ExpiresIn = 120.0;
		private static readonly int m_ArmingSkill = 25;
		private static readonly int m_DisarmingSkill = 40;
		private static readonly int m_KarmaLoss = 40;
		private static readonly bool m_AllowedInTown = false;

		[Constructable]
		public ExplosiveLesserTrap()
			: base(m_ArmedName, m_UnarmedName, m_ExpiresIn, m_ArmingSkill, m_DisarmingSkill, m_KarmaLoss,
				m_AllowedInTown)
		{
		}

		public override void TrapEffect(Mobile from)
		{
			from.PlaySound(0x4A); // click sound

			from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
			from.PlaySound(0x307);

			int damage = Utility.RandomMinMax(20, 40);
			AOS.Damage(from, from, damage, 100, 100, 0, 0, 0);

			bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
			if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
				((PlayerMobile)this.Owner).TrapsActive -= 1;

			this.Delete();
		}

		public ExplosiveLesserTrap(Serial serial) : base(serial)
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

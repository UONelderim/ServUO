//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//

#region References

using System;
using Server.Mobiles;

#endregion

namespace Server.Items
{
	public class ParalysisRegularTrap : BaseTinkerTrap
	{
		private static readonly string m_ArmedName = "uzbrojona paraliżująca pułapka";
		private static readonly string m_UnarmedName = "nieuzbrojona paraliżująca pułapka";
		private static readonly double m_ExpiresIn = 120.0;
		private static readonly int m_ArmingSkill = 25;
		private static readonly int m_DisarmingSkill = 50;
		private static readonly int m_KarmaLoss = 0;
		private static readonly bool m_AllowedInTown = false;

		[Constructable]
		public ParalysisRegularTrap()
			: base(m_ArmedName, m_UnarmedName, m_ExpiresIn, m_ArmingSkill, m_DisarmingSkill, m_KarmaLoss,
				m_AllowedInTown)
		{
		}

		public override void TrapEffect(Mobile from)
		{
			from.PlaySound(0x4A); // click sound

			from.PlaySound(0x204);
			from.FixedEffect(0x376A, 6, 1);

			int duration = Utility.RandomMinMax(3, 6);
			from.Paralyze(TimeSpan.FromSeconds(duration));

			bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
			if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
				((PlayerMobile)this.Owner).TrapsActive -= 1;

			this.Delete();
		}

		public ParalysisRegularTrap(Serial serial) : base(serial)
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

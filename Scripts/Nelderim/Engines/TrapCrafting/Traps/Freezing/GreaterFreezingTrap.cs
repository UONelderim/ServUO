//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//
using System;
using Server.Mobiles;

namespace Server.Items
{
    public class FreezingGreaterTrap : BaseTinkerTrap
    {
        private Boolean m_TrapArmed = false;
        private DateTime m_TimeTrapArmed;

        private static string m_ArmedName = "uzbrojona powiększa zamrażająca pułapka";
        private static string m_UnarmedName = "nieuzbrojona powiększa zamrażająca pułapka";
		private static double m_ExpiresIn = 900.0;
        private static int m_DisarmingSkill = 60;
        private static int m_KarmaLoss = 60;
        private static bool m_AllowedInTown = false;

        [Constructable]
        public FreezingGreaterTrap()
            : base(m_ArmedName, m_UnarmedName, m_ExpiresIn, m_DisarmingSkill, m_KarmaLoss, m_AllowedInTown)
        {
        }

        public override void TrapEffect(Mobile from)
        {
            from.PlaySound(0x4A);  // click sound

            from.PlaySound(0x204);
            from.FixedEffect(0x376A, 6, 1);

            int duration = Utility.RandomMinMax(4, 8);
            int damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(from, from, damage, 0, 0, 100, 0, 0);
            from.Paralyze(TimeSpan.FromSeconds(duration));

            bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
            if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
                ((PlayerMobile)this.Owner).TrapsActive -= 1;

            this.Delete();
        }

        public FreezingGreaterTrap(Serial serial) : base(serial)
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

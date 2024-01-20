//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//
using System;
using Server.Mobiles;

namespace Server.Items
{
    public class TrapTest : BaseTinkerTrap
    {
        private Boolean m_TrapArmed = false;
        private DateTime m_TimeTrapArmed;

        private static string m_ArmedName = "uzbrojony tester pułapek";
        private static string m_UnarmedName = "nieuzbrojony tester pułapek";
        private static double m_ExpiresIn = 300.0;
        private static int m_DisarmingSkill = 10;
        private static int m_KarmaLoss = 0;
        private static bool m_AllowedInTown = true;

        [Constructable]
        public TrapTest()
            : base(m_ArmedName, m_UnarmedName, m_ExpiresIn, m_DisarmingSkill, m_KarmaLoss, m_AllowedInTown)
        {
        }

        public override void TrapEffect(Mobile from)
        {
            //
            // Insert Effects here (Varies depending on the Trap)
            //
            from.PlaySound(0x4A);  // click sound
            from.PlaySound(0x5C);  // fizzle sound

            from.SendMessage("Oops");

            bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
            if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
                ((PlayerMobile)this.Owner).TrapsActive -= 1;

            this.Delete();
        }

        public TrapTest(Serial serial) : base(serial)
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

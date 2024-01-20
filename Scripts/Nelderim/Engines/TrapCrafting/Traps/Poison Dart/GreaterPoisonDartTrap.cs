//
// ** Basic Trap Framework (BTF)
// ** Trap Test Script (poison dart trap)
// ** Author: Lichbane
//
using System;
using Server.Mobiles;

namespace Server.Items
{
    public class PoisonGreaterDartTrap : BaseTinkerTrap
    {
        private Boolean m_TrapArmed = false;
        private DateTime m_TimeTrapArmed;

        private static string m_ArmedName = "uzbrojona trująca pułapka z mocną trucizną";
        private static string m_UnarmedName = "nieuzbrojona trująca pułapka z mocną trucizną";
        private static double m_ExpiresIn = 900.0;
        private static int m_DisarmingSkill = 50;
        private static int m_KarmaLoss = 100;
        private static bool m_AllowedInTown = false;

        [Constructable]
        public PoisonGreaterDartTrap()
            : base(m_ArmedName, m_UnarmedName, m_ExpiresIn, m_DisarmingSkill, m_KarmaLoss, m_AllowedInTown)
        {
        }

        public override void TrapEffect(Mobile from)
        {
            from.PlaySound(0x4A);  // click sound

            if (from.Alive == true)
            {
                double penetration = Utility.RandomMinMax(20, 200);
                if (from.ArmorRating > penetration)
                {
                    from.SendMessage("A poison dart bounces off your armor.");
                }
                else
                {
                    from.ApplyPoison(from, Poison.Greater);
                    from.SendMessage("You feel the sting of a poison dart");
                }
            }
            bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;
            if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
                ((PlayerMobile)this.Owner).TrapsActive -= 1;

            this.Delete();
        }

        public PoisonGreaterDartTrap(Serial serial) : base(serial)
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

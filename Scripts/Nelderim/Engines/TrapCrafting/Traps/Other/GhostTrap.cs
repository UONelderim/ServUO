//
// ** Basic Trap Framework (BTF)
// ** Author: Lichbane
//
using System;
using Server.Mobiles;

namespace Server.Items
{
    public class GhostTrap : BaseTinkerTrap
    {
        private int m_MinResist = 70;  // Minimum MagicResist. Below is Auto Captured.
        private int m_MaxResist = 120;  // Maximum MagicResist. Above is AutoResist. 

        private Boolean m_TrapArmed = false;
        private DateTime m_TimeTrapArmed;

        private static string m_ArmedName = "uzbrojona pułapka z duchem";
        private static string m_UnarmedName = "nieuzbrojona pułapka z duchem";
        private static double m_ExpiresIn = 900.0;
        private static int m_DisarmingSkill = 0;
        private static int m_KarmaLoss = 0;
        private static bool m_AllowedInTown = true;

        [Constructable]
        public GhostTrap()
            : base(m_ArmedName, m_UnarmedName, m_ExpiresIn, m_DisarmingSkill, m_KarmaLoss, m_AllowedInTown)
        {
        }

        public override void ArmTrap(Mobile from)
        {
            this.Visible = true;  // Make sure the trap is visible.
        }

        public override void TrapCheckTrigger(Mobile from)
        {
            if ( from.Body == 0x99 || from.Body == 0x1A )  // Body Types for Ghost Type Mobiles
                if ((Utility.Random(100) + 1) > from.Skills.MagicResist.Base)
                    TrapEffect( from );
                else
                    from.PlaySound(0x4A);
        }

        public override void TrapEffect(Mobile from)
        {
            //
            // Insert Effects here (Varies depending on the Trap)
            //
            Point3D loc = this.Location;
            Map map = this.Map;
            Item item = new TrappedGhost();

            Effects.SendLocationParticles(EffectItem.Create( loc, map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 0); 
            from.PlaySound(0x208);
            bool m_TrapsLimit = Trapcrafting.Config.TrapsLimit;

            if ((m_TrapsLimit) && (((PlayerMobile)this.Owner).TrapsActive > 0))
                ((PlayerMobile)this.Owner).TrapsActive -= 1;

            this.Delete();  

            if (!from.Player)   // Just in case (and it should NEVER happen), but ....
            {
                from.Delete();  // ... make sure Players are isolated from this bit of code. BAD!!
                item.MoveToWorld(loc, map);
            }
        }

        public GhostTrap(Serial serial) : base(serial)
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

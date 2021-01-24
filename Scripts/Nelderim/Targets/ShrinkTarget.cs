using Server.Items;
using Server.Mobiles;
using Server.Targeting;

namespace Server.Targets
{
    public class ShrinkTarget : Target
    {
        private BaseVendor m_Trainer;

        public ShrinkTarget( BaseVendor trainer ) : base( 12, false, TargetFlags.None )
        {
            m_Trainer = trainer;
        }

        protected override void OnTarget( Mobile from, object targeted )
        {
            ShrunkPet.Shrink( m_Trainer, from as PlayerMobile, targeted as BaseCreature );
        }
    }
}

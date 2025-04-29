using System;
using Server.Mobiles;

namespace Server.Items
{
    public class BladeSpiritTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 50;
        protected override int KarmaLoss => 120;
        protected override bool AllowedInTown => false;

        [Constructable]
        public BladeSpiritTrap()
        {
	        Name = "Pułapka z Duchem Ostrzy";
        }

        public override void TrapEffect(Mobile from)
        {
            if (Owner != null)
            {
                BaseCreature.Summon(new BladeSpirits(), false, Owner, Location, 0x212, TimeSpan.FromSeconds(60));
            }
        }

        public BladeSpiritTrap(Serial serial) : base(serial)
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

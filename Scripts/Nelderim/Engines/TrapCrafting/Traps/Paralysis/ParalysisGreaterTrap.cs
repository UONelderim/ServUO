using System;

namespace Server.Items
{
    public class ParalysisGreaterTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 60;
        protected override int KarmaLoss => 0;
        protected override bool AllowedInTown => false;

        [Constructable]
        public ParalysisGreaterTrap()
        {
	        Name = "Większa Paraliżująca Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.PlaySound(0x204);
            from.FixedEffect(0x376A, 6, 1);

            var duration = Utility.RandomMinMax(4, 8);
            from.Paralyze(TimeSpan.FromSeconds(duration));
        }

        public ParalysisGreaterTrap(Serial serial) : base(serial)
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

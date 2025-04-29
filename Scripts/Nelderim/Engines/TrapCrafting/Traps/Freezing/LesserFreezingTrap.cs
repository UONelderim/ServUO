using System;

namespace Server.Items
{
    public class FreezingLesserTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 40;
        protected override int KarmaLoss => 40;
        protected override bool AllowedInTown => false;

        [Constructable]
        public FreezingLesserTrap()
        {
	        Name = "Mniejsza Zamrażająca Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.PlaySound(0x204);
            from.FixedEffect(0x376A, 6, 1);

            var damage = Utility.RandomMinMax(20, 40);
            AOS.Damage(from, from, damage, 0, 0, 100, 0, 0);
            var duration = Utility.RandomMinMax(2, 4);
            from.Paralyze(TimeSpan.FromSeconds(duration));
        }

        public FreezingLesserTrap(Serial serial) : base(serial)
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

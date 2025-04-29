using System;

namespace Server.Items
{
    public class FreezingRegularTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 50;
        protected override int KarmaLoss => 50;
        protected override bool AllowedInTown => false;

        [Constructable]
        public FreezingRegularTrap()
        {
	        Name = "Zamrażająca Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.PlaySound(0x204);
            from.FixedEffect(0x376A, 6, 1);

            var damage = Utility.RandomMinMax(30, 50);
            AOS.Damage(from, from, damage, 0, 0, 100, 0, 0);
            var duration = Utility.RandomMinMax(3, 6);
            from.Paralyze(TimeSpan.FromSeconds(duration));
        }

        public FreezingRegularTrap(Serial serial) : base(serial)
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

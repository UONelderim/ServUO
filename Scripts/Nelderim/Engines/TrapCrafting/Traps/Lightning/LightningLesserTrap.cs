namespace Server.Items
{
    public class LightningLesserTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 40;
        protected override int KarmaLoss => 40;
        protected override bool AllowedInTown => false;

        [Constructable]
        public LightningLesserTrap()
        {
	        Name = "Mniejsza Porażająca Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.BoltEffect(0);

            var damage = Utility.RandomMinMax(20, 40);
            AOS.Damage(from, from, damage, 50, 0, 0, 0, 100);
        }

        public LightningLesserTrap(Serial serial) : base(serial)
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

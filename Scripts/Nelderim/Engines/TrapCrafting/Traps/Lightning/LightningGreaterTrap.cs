namespace Server.Items
{
    public class LightningGreaterTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 60;
        protected override int KarmaLoss => 60;
        protected override bool AllowedInTown => false;

        [Constructable]
        public LightningGreaterTrap()
        {
	        Name = "Większa Porażająca Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.BoltEffect(0);

            var damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(from, from, damage, 50, 0, 0, 0, 100);
        }

        public LightningGreaterTrap(Serial serial) : base(serial)
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

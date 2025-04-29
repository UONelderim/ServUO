namespace Server.Items
{
    public class LightningRegularTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 50;
        protected override int KarmaLoss => 50;
        protected override bool AllowedInTown => false;

        [Constructable]
        public LightningRegularTrap()
        {
	        Name = "Porażająca Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.BoltEffect(0);

            var damage = Utility.RandomMinMax(30, 50);
            AOS.Damage(from, from, damage, 50, 0, 0, 0, 100);
        }

        public LightningRegularTrap(Serial serial) : base(serial)
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

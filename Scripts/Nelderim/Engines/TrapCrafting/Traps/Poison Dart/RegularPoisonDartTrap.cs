namespace Server.Items
{
    public class PoisonRegularDartTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 40;
        protected override int KarmaLoss => 80;
        protected override bool AllowedInTown => false;

        [Constructable]
        public PoisonRegularDartTrap()
        {
	        Name = "Trująca Pułapka z Trucizną";
        }

        public override void TrapEffect(Mobile from)
        {
            if (from.Alive)
                from.ApplyPoison(from, Poison.Regular);
        }

        public PoisonRegularDartTrap(Serial serial) : base(serial)
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

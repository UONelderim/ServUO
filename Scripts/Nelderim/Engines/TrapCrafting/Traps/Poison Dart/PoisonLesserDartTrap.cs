namespace Server.Items
{
    public class PoisonLesserDartTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 30;
        protected override int KarmaLoss => 60;
        protected override bool AllowedInTown => false;

        [Constructable]
        public PoisonLesserDartTrap()
        {
	        Name = "Trująca Pułapka ze Lekką Trucizną";
        }

        public override void TrapEffect(Mobile from)
        {
            if (from.Alive)
                from.ApplyPoison(from, Poison.Lesser);
        }

        public PoisonLesserDartTrap(Serial serial) : base(serial)
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

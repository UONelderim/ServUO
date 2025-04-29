namespace Server.Items
{
    public class PoisonGreaterDartTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 50;
        protected override int KarmaLoss => 100;
        protected override bool AllowedInTown => false;

        [Constructable]
        public PoisonGreaterDartTrap()
        {
	        Name = "Trująca Pułapka ze Mocną Trucizną";
        }

        public override void TrapEffect(Mobile from)
        {
            if (from.Alive )
                from.ApplyPoison(from, Poison.Greater);
        }

        public PoisonGreaterDartTrap(Serial serial) : base(serial)
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

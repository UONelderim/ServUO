namespace Server.Items
{
    public class PoisonDeadlyDartTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 60;
        protected override int KarmaLoss => 120;
        protected override bool AllowedInTown => false;

        [Constructable]
        public PoisonDeadlyDartTrap()
        {
	        Name = "Trująca Pułapka ze Śmiertelną Trucizną";
        }

        public override void TrapEffect(Mobile from)
        {
            if (from.Alive)
                from.ApplyPoison(from, Poison.Deadly);
        }

        public PoisonDeadlyDartTrap(Serial serial) : base(serial)
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

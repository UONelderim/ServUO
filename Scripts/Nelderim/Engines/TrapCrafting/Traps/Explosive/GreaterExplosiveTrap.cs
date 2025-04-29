namespace Server.Items
{
    public class ExplosiveGreaterTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 60;
        protected override int KarmaLoss => 60;
        protected override bool AllowedInTown => false;

        [Constructable]
        public ExplosiveGreaterTrap()
        {
	        Name = "Większa Wybuchowa Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
            from.PlaySound(0x307);

            var damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(from, from, damage, 100, 100, 0, 0, 0);
        }

        public ExplosiveGreaterTrap(Serial serial) : base(serial)
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

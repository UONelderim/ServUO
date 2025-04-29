namespace Server.Items
{
    public class ExplosiveRegularTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 50;
        protected override int KarmaLoss => 50;
        protected override bool AllowedInTown => false;

        [Constructable]
        public ExplosiveRegularTrap()
        {
	        Name = "Wybuchowa Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
            from.PlaySound(0x307);

            var damage = Utility.RandomMinMax(30, 50);
            AOS.Damage(from, from, damage, 100, 100, 0, 0, 0);
        }

        public ExplosiveRegularTrap(Serial serial) : base(serial)
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

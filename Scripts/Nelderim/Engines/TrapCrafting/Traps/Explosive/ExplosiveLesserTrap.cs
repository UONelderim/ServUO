namespace Server.Items
{
    public class ExplosiveLesserTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 40;
        protected override int KarmaLoss => 40;
        protected override bool AllowedInTown => false;

        [Constructable]
        public ExplosiveLesserTrap()
        {
	        Name = "Mniejsza Wybuchowa Pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
            from.PlaySound(0x307);

            var damage = Utility.RandomMinMax(20, 40);
            AOS.Damage(from, from, damage, 0, 100, 0, 0, 0);
        }

        public ExplosiveLesserTrap(Serial serial) : base(serial)
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

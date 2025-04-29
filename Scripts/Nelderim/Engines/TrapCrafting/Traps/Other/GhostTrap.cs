namespace Server.Items
{
    public class GhostTrap : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 0;
        protected override int KarmaLoss => 0;
        protected override bool AllowedInTown => true;

        [Constructable]
        public GhostTrap()
        {
	        Name = "Pułapka na Duchy";
        }

        public override void OnTrapArmed(Mobile from)
        {
	        base.OnTrapArmed(from);
            Visible = true;  // Make sure the trap is visible.
        }

        public override bool CheckTrigger(Mobile from)
        {
	        if (from.Body == 0x99 || from.Body == 0x1A)
	        {
		        return (Utility.Random(100) + 1) > from.Skills.MagicResist.Base;
	        }
	        return false;
        }

        public override void TrapEffect(Mobile from)
        {
            Effects.SendLocationParticles(EffectItem.Create( Location, Map, EffectItem.DefaultDuration ), 0x3709, 10, 30, 0); 
            from.PlaySound(0x208);

            if (!from.Player) // Just in case (and it should NEVER happen), but ....
            {
                from.Delete();  // ... make sure Players are isolated from this bit of code. BAD!!
                Item item = new TrappedGhost();
                item.MoveToWorld(Location, Map);
            }
        }

        public GhostTrap(Serial serial) : base(serial)
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

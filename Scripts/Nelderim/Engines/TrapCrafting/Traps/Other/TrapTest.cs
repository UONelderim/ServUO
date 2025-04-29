using Server.Mobiles;

namespace Server.Items
{
    public class TrapTest : BaseTinkerTrap
    {
	    public override int DisarmingSkillReq => 10;
        protected override int KarmaLoss => 0;
        protected override bool AllowedInTown => true;

        [Constructable]
        public TrapTest()
        {
	        Name = "Testowa pułapka";
        }

        public override void TrapEffect(Mobile from)
        {
            from.PlaySound(0x5C); 
            from.SendMessage("Aktywacja");
        }

        public TrapTest(Serial serial) : base(serial)
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

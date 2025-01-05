namespace Server.Items
{
    public class BeaconOfHope : MetalShield
    {
		public override int BaseFireResistance => 10;
		public override int InitMinHits => 60;
		public override int InitMaxHits => 60;

		[Constructable]
        public BeaconOfHope()
        {
            Name = "Wskaznik Nadziei";
			ItemID = 0xA25;
            Hue = 1767;
            
            StrRequirement = 55;
            Attributes.SpellChanneling = 1;
            Attributes.AttackChance = 12;
            Attributes.DefendChance = 12;
			Attributes.CastSpeed = 1;
			Attributes.BonusMana = 10;
			Attributes.BonusStam = -12;
            Attributes.Luck = -200;
        }

        public BeaconOfHope(Serial serial) : base( serial )
        {
        }

		public override void OnDoubleClick( Mobile from )
		{
			if ( !IsChildOf( from.Backpack ) && Parent != from )
			{
				from.SendLocalizedMessage( 500364 ); // To nalezy do kogos innego.
				return;
			}
			if ( ItemID == 2597 )
				ItemID = 2594;
			else
				ItemID = 2597;
		}
		
        public override void Serialize( GenericWriter writer )
        {
            base.Serialize( writer );
            writer.Write( (int) 0 );
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize( reader );
            int version = reader.ReadInt();
        }
    }
}

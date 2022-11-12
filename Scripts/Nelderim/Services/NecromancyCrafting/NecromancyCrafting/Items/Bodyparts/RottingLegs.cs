namespace Server.Items
{
	public class RottingLegs : Item
	{
		public override string DefaultName
		{
			get { return "gnijące nogi"; }
		}

		[Constructable]
		public RottingLegs() : base( 0x1CDF )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public RottingLegs( Serial serial ) : base( serial )
		{
		}

		public override void Serialize( GenericWriter writer )
		{
			base.Serialize( writer );

			writer.Write( (int) 0 );
		}

		public override void Deserialize( GenericReader reader )
		{
			base.Deserialize( reader );

			int version = reader.ReadInt();
		}
	}
}
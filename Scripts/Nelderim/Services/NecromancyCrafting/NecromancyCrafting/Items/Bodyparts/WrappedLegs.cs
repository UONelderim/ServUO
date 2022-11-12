namespace Server.Items
{
	public class WrappedLegs : Item
	{
		public override string DefaultName
		{
			get { return "zmumifikowane nogi"; }
		}

		[Constructable]
		public WrappedLegs() : base( 0x1D8B )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public WrappedLegs( Serial serial ) : base( serial )
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
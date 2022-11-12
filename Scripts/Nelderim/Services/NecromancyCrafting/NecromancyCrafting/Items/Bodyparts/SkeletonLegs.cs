namespace Server.Items
{
	public class SkeletonLegs : Item
	{
		public override string DefaultName
		{
			get { return "nogi szkieleta"; }
		}

		[Constructable]
		public SkeletonLegs() : base( 0x1D90 )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public SkeletonLegs( Serial serial ) : base( serial )
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
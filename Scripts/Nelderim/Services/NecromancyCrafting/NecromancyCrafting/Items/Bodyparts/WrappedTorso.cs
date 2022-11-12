namespace Server.Items
{
	public class WrappedTorso : Item
	{
		public override string DefaultName
		{
			get { return "zmumifikowany tułów"; }
		}

		[Constructable]
		public WrappedTorso() : base( 0x1D8A )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public WrappedTorso( Serial serial ) : base( serial )
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
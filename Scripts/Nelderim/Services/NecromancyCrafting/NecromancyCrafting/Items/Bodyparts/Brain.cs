namespace Server.Items
{
	public class Brain : Item
	{
		public override string DefaultName
		{
			get { return "mózg"; }
		}

		[Constructable]
		public Brain() : base( 0x1CF0 )
		{
			Weight = 1.0;
			Stackable = true;
		}

		public Brain( Serial serial ) : base( serial )
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